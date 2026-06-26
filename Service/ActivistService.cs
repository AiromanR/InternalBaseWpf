using System;
using System.Collections.Generic;
using System.Linq;
using InternalBaseWpf.Data;
using InternalBaseWpf.Models;
using Microsoft.EntityFrameworkCore;

namespace InternalBaseWpf.Service
{
    public class ActivistService
    {
        private readonly AppDbContext _db = DBService.Instance.Context;

        public List<Activist> GetAll(bool confirmedOnly = true)
        {
            var query = _db.Activists
                .Include(a => a.MainCell)
                .Include(a => a.ResponsibleUser)
                .Include(a => a.ActivistCells)
                .ThenInclude(ac => ac.Cell)
                .AsQueryable();

            if (confirmedOnly)
                query = query.Where(a => a.IsConfirmed);

            return query.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToList();
        }

        public List<Activist> GetUnconfirmed()
        {
            return _db.Activists
                .Include(a => a.MainCell)
                .Include(a => a.ActivistCells)
                .ThenInclude(ac => ac.Cell)
                .Where(a => !a.IsConfirmed)
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .ToList();
        }

        public Activist? GetById(int id)
        {
            return _db.Activists
                .Include(a => a.ActivistCells)
                .ThenInclude(ac => ac.Cell)
                .FirstOrDefault(a => a.Id == id);
        }

        public void Add(Activist activist)
        {
            _db.Activists.Add(activist);
            _db.SaveChanges();
        }

        public void Update(Activist activist)
        {
            var existing = _db.Activists
                .Include(a => a.ActivistCells)
                .FirstOrDefault(a => a.Id == activist.Id);

            if (existing == null)
                throw new InvalidOperationException("Активист не найден");

            _db.Entry(existing).CurrentValues.SetValues(activist);

            var existingCells = existing.ActivistCells.ToList();
            foreach (var ac in existingCells)
            {
                existing.ActivistCells.Remove(ac);
                _db.ActivistCells.Remove(ac);
            }

            foreach (var ac in activist.ActivistCells)
            {
                _db.ActivistCells.Add(new ActivistCell
                {
                    ActivistId = existing.Id,
                    CellId = ac.CellId
                });
            }

            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var activist = _db.Activists.Find(id);
            if (activist == null)
                return;

            _db.Activists.Remove(activist);
            _db.SaveChanges();
        }

        public void Confirm(int id)
        {
            var activist = _db.Activists.Find(id);
            if (activist == null)
                return;

            activist.IsConfirmed = true;
            _db.SaveChanges();
        }

        public bool PhoneExists(string? phone, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            var query = _db.Activists.Where(a => a.Phone == phone);
            if (excludeId.HasValue)
                query = query.Where(a => a.Id != excludeId.Value);

            return query.Any();
        }

        public List<Activist> GetDuplicates()
        {
            return _db.Activists
                .AsEnumerable()
                .GroupBy(a => new
                {
                    a.LastName,
                    a.FirstName,
                    a.Patronymic,
                    a.BirthDate
                })
                .Where(g => g.Count() > 1)
                .SelectMany(g => g)
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .ToList();
        }

        public List<Activist> Filter(ActivistFilter filter)
        {
            var query = _db.Activists
                .Include(a => a.MainCell)
                .Include(a => a.ResponsibleUser)
                .Include(a => a.ActivistCells)
                .ThenInclude(ac => ac.Cell)
                .Where(a => a.IsConfirmed)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.LastName))
                query = query.Where(a => a.LastName.Contains(filter.LastName));

            if (!string.IsNullOrWhiteSpace(filter.FirstName))
                query = query.Where(a => a.FirstName.Contains(filter.FirstName));

            if (!string.IsNullOrWhiteSpace(filter.Patronymic))
                query = query.Where(a => a.Patronymic != null && a.Patronymic.Contains(filter.Patronymic));

            if (!string.IsNullOrWhiteSpace(filter.Gender))
                query = query.Where(a => a.Gender == filter.Gender);

            if (!string.IsNullOrWhiteSpace(filter.Phone))
                query = query.Where(a => a.Phone != null && a.Phone.Contains(filter.Phone));

            if (!string.IsNullOrWhiteSpace(filter.Address))
                query = query.Where(a => a.Address != null && a.Address.Contains(filter.Address));

            if (!string.IsNullOrWhiteSpace(filter.LocalBranch))
                query = query.Where(a => a.LocalBranch == filter.LocalBranch);

            if (!string.IsNullOrWhiteSpace(filter.PrimaryBranch))
                query = query.Where(a => a.PrimaryBranch == filter.PrimaryBranch);

            if (!string.IsNullOrWhiteSpace(filter.UikNumber))
                query = query.Where(a => a.UikNumber == filter.UikNumber);

            if (!string.IsNullOrWhiteSpace(filter.Loyalty))
                query = query.Where(a => a.Loyalty == filter.Loyalty);

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var text = filter.SearchText.ToLower();
                query = query.Where(a =>
                    a.LastName.ToLower().Contains(text) ||
                    a.FirstName.ToLower().Contains(text) ||
                    (a.Patronymic != null && a.Patronymic.ToLower().Contains(text)) ||
                    (a.Phone != null && a.Phone.Contains(text)));
            }

            if (filter.MainCellId.HasValue)
                query = query.Where(a => a.MainCellId == filter.MainCellId.Value);

            if (filter.YoungerThan18)
            {
                var date = DateTime.Today.AddYears(-18);
                query = query.Where(a => a.BirthDate > date);
            }

            if (filter.OlderThan100)
            {
                var date = DateTime.Today.AddYears(-100);
                query = query.Where(a => a.BirthDate < date);
            }

            return query.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToList();
        }
    }

    public class ActivistFilter
    {
        public string? SearchText { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Patronymic { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? LocalBranch { get; set; }
        public string? PrimaryBranch { get; set; }
        public string? UikNumber { get; set; }
        public string? Loyalty { get; set; }
        public int? MainCellId { get; set; }
        public bool YoungerThan18 { get; set; }
        public bool OlderThan100 { get; set; }
    }
}
