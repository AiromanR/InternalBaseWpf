using System.Collections.Generic;
using System.Linq;
using InternalBaseWpf.Data;
using InternalBaseWpf.Models;

namespace InternalBaseWpf.Service
{
    public class CellService
    {
        private readonly AppDbContext _db = DBService.Instance.Context;

        public List<Cell> GetAll()
        {
            return _db.Cells.OrderBy(c => c.Name).ToList();
        }

        public Cell? GetById(int id)
        {
            return _db.Cells.Find(id);
        }

        public void Add(Cell cell)
        {
            _db.Cells.Add(cell);
            _db.SaveChanges();
        }

        public void Update(Cell cell)
        {
            _db.Cells.Update(cell);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var cell = _db.Cells.Find(id);
            if (cell == null)
                return;

            _db.Cells.Remove(cell);
            _db.SaveChanges();
        }

        public List<Cell> Search(string query)
        {
            query = query.ToLower();
            return _db.Cells
                .Where(c => c.Name.ToLower().Contains(query))
                .OrderBy(c => c.Name)
                .ToList();
        }
    }
}
