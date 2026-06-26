using System;
using System.Collections.Generic;
using System.Linq;
using InternalBaseWpf.Data;
using InternalBaseWpf.Models;

namespace InternalBaseWpf.Service
{
    public class TouchService
    {
        private readonly AppDbContext _db = DBService.Instance.Context;

        public List<Touch> GetAll(bool includeCompleted = false)
        {
            var query = _db.Touches.AsQueryable();
            if (!includeCompleted)
                query = query.Where(t => t.EndDate == null || t.EndDate >= DateTime.Today);

            return query.OrderByDescending(t => t.CreatedAt).ToList();
        }

        public Touch? GetById(int id)
        {
            return _db.Touches.Find(id);
        }

        public void Add(Touch touch)
        {
            _db.Touches.Add(touch);
            _db.SaveChanges();
        }

        public void Update(Touch touch)
        {
            _db.Touches.Update(touch);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var touch = _db.Touches.Find(id);
            if (touch == null)
                return;

            _db.Touches.Remove(touch);
            _db.SaveChanges();
        }

        public List<TouchType> GetTypes()
        {
            return _db.TouchTypes.OrderBy(t => t.Name).ToList();
        }
    }
}
