using System;

namespace InternalBaseWpf.Models
{
    public class Touch
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? MediaLink { get; set; }
        public string? SocialLink { get; set; }
        public int? Coverage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? TypeId { get; set; }
        public virtual TouchType? Type { get; set; }

        public int? CellId { get; set; }
        public virtual Cell? Cell { get; set; }

        public int? OperatorId { get; set; }
        public virtual User? Operator { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsShopPlaced { get; set; }
        public bool IsRikPlaced { get; set; }
        public bool HasPhoto { get; set; }
        public bool HasPublication { get; set; }
        public bool IsCuratorApproved { get; set; }

        public string ResultText => "нет результатов";
    }
}
