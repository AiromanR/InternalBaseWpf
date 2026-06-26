using System;
using System.Collections.Generic;

namespace InternalBaseWpf.Models
{
    public class Cell
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<Activist> MainActivists { get; set; } = new List<Activist>();
        public virtual ICollection<ActivistCell> ActivistCells { get; set; } = new List<ActivistCell>();
        public virtual ICollection<Touch> Touches { get; set; } = new List<Touch>();
    }
}
