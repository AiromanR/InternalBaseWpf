using System;
using System.Collections.Generic;

namespace InternalBaseWpf.Models
{
    public class Activist
    {
        public int Id { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string FullName => $"{LastName} {FirstName} {Patronymic}".Trim();
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        public string? LocalBranch { get; set; }
        public string? PrimaryBranch { get; set; }
        public string? UikNumber { get; set; }

        public bool IsResponsible { get; set; }
        public bool CanEdit { get; set; }
        public int? ResponsibleUserId { get; set; }
        public virtual User? ResponsibleUser { get; set; }

        public int? MainCellId { get; set; }
        public virtual Cell? MainCell { get; set; }

        public virtual ICollection<ActivistCell> ActivistCells { get; set; } = new List<ActivistCell>();

        public string? PartyStatus { get; set; }
        public string? OrgStatus { get; set; }
        public string? Workplace { get; set; }
        public string? Position { get; set; }
        public string? Note { get; set; }

        public bool IsPartySupporter { get; set; }
        public bool IsPresidentSupporter { get; set; }
        public bool IsVdlSupporter { get; set; }

        public string? Loyalty { get; set; }
        public string? TargetAudience { get; set; }
        public string? CitizenshipCategory { get; set; }

        public bool IsAts { get; set; }
        public bool IsVverh { get; set; }
        public string? Readiness { get; set; }
        public string? RegionalReadiness { get; set; }

        public bool MarkedForDeletionByResponsible { get; set; }
        public string? DeletionReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsConfirmed { get; set; } = true;
        public string? ImportStatus { get; set; }
    }
}
