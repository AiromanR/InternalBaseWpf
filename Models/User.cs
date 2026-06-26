using System;

namespace InternalBaseWpf.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
