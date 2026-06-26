namespace InternalBaseWpf.Models
{
    public class ActivistCell
    {
        public int ActivistId { get; set; }
        public virtual Activist Activist { get; set; } = null!;

        public int CellId { get; set; }
        public virtual Cell Cell { get; set; } = null!;
    }
}
