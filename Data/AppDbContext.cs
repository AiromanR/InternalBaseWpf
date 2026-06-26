using System;
using InternalBaseWpf.Models;
using InternalBaseWpf.Service;
using Microsoft.EntityFrameworkCore;

namespace InternalBaseWpf.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Activist> Activists { get; set; } = null!;
        public virtual DbSet<Cell> Cells { get; set; } = null!;
        public virtual DbSet<Touch> Touches { get; set; } = null!;
        public virtual DbSet<TouchType> TouchTypes { get; set; } = null!;
        public virtual DbSet<ActivistCell> ActivistCells { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=localhost\\SQLEXPRESS;Database=InternalBaseWpf;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivistCell>()
                .HasKey(ac => new { ac.ActivistId, ac.CellId });

            modelBuilder.Entity<ActivistCell>()
                .HasOne(ac => ac.Activist)
                .WithMany(a => a.ActivistCells)
                .HasForeignKey(ac => ac.ActivistId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ActivistCell>()
                .HasOne(ac => ac.Cell)
                .WithMany(c => c.ActivistCells)
                .HasForeignKey(ac => ac.CellId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Activist>()
                .HasOne(a => a.ResponsibleUser)
                .WithMany()
                .HasForeignKey(a => a.ResponsibleUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Activist>()
                .HasOne(a => a.MainCell)
                .WithMany(c => c.MainActivists)
                .HasForeignKey(a => a.MainCellId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Touch>()
                .HasOne(t => t.Type)
                .WithMany()
                .HasForeignKey(t => t.TypeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Touch>()
                .HasOne(t => t.Cell)
                .WithMany(c => c.Touches)
                .HasForeignKey(t => t.CellId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Touch>()
                .HasOne(t => t.Operator)
                .WithMany()
                .HasForeignKey(t => t.OperatorId)
                .OnDelete(DeleteBehavior.SetNull);

            var (adminHash, adminSalt) = PasswordService.HashPassword("admin");
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Login = "admin",
                PasswordHash = adminHash,
                Salt = adminSalt,
                FullName = "Администратор",
                IsAdmin = true,
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1)
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 2,
                Login = "rogova",
                PasswordHash = adminHash,
                Salt = adminSalt,
                FullName = "Ирина Рогова",
                IsAdmin = false,
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1)
            });

            modelBuilder.Entity<TouchType>().HasData(
                new TouchType { Id = 1, Name = "Оффлайн мероприятие" },
                new TouchType { Id = 2, Name = "Онлайн мероприятие" },
                new TouchType { Id = 3, Name = "Иное" });

            modelBuilder.Entity<Cell>().HasData(
                new Cell { Id = 1, Name = "МО Куединского р-на", IsActive = true },
                new Cell { Id = 2, Name = "Куединское РДК", IsActive = true },
                new Cell { Id = 3, Name = "Сайт Конституция МО Куединского муниципального округа", IsActive = true },
                new Cell { Id = 4, Name = "Сторонники МО Куединского муниципального округа", IsActive = true },
                new Cell { Id = 5, Name = "Молодёжный совет Куединского МО", IsActive = true },
                new Cell { Id = 6, Name = "Совет ветеранов Куединского МО", IsActive = true },
                new Cell { Id = 7, Name = "ТИК Куединского МО", IsActive = true },
                new Cell { Id = 8, Name = "Общественная палата Куединского МО", IsActive = true },
                new Cell { Id = 9, Name = "Центральная библиотека Куединского МО", IsActive = true },
                new Cell { Id = 10, Name = "Краеведческий музей Куединского МО", IsActive = true },
                new Cell { Id = 11, Name = "Спортивный клуб Куединского МО", IsActive = true },
                new Cell { Id = 12, Name = "Волонтёрский центр Куединского МО", IsActive = true },
                new Cell { Id = 13, Name = "Куединский районный Дом культуры", IsActive = true });

            modelBuilder.Entity<Activist>().HasData(
                new Activist
                {
                    Id = 1,
                    LastName = "Иванов",
                    FirstName = "Иван",
                    Patronymic = "Иванович",
                    Gender = "Мужской",
                    BirthDate = new DateTime(1980, 5, 15),
                    Phone = "(912) 345-67-89",
                    Address = "г. Пермь, ул. Ленина, д. 1",
                    LocalBranch = "МО Куединского муниципального округа",
                    UikNumber = "1822",
                    IsConfirmed = true,
                    CreatedAt = new DateTime(2024, 1, 15)
                },
                new Activist
                {
                    Id = 2,
                    LastName = "Петрова",
                    FirstName = "Мария",
                    Patronymic = "Сергеевна",
                    Gender = "Женский",
                    BirthDate = new DateTime(1992, 8, 22),
                    Phone = "(922) 456-78-90",
                    Address = "г. Пермь, ул. Гагарина, д. 5",
                    LocalBranch = "МО Куединского муниципального округа",
                    UikNumber = "1823",
                    MainCellId = 2,
                    IsConfirmed = true,
                    CreatedAt = new DateTime(2024, 2, 10)
                },
                new Activist
                {
                    Id = 3,
                    LastName = "Иванов",
                    FirstName = "Иван",
                    Patronymic = "Иванович",
                    Gender = "Мужской",
                    BirthDate = new DateTime(1980, 5, 15),
                    Phone = "(912) 345-67-99",
                    Address = "г. Пермь, ул. Ленина, д. 2",
                    LocalBranch = "МО Куединского муниципального округа",
                    UikNumber = "1822",
                    IsConfirmed = true,
                    CreatedAt = new DateTime(2024, 3, 5)
                });

            modelBuilder.Entity<Touch>().HasData(
                new Touch
                {
                    Id = 1,
                    Name = "День России",
                    TypeId = 1,
                    CellId = 2,
                    OperatorId = 2,
                    StartDate = new DateTime(2026, 6, 12),
                    EndDate = new DateTime(2026, 6, 12),
                    Coverage = 40,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 5, 26)
                },
                new Touch
                {
                    Id = 2,
                    Name = "День молодёжи",
                    TypeId = 1,
                    CellId = 2,
                    OperatorId = 2,
                    StartDate = new DateTime(2026, 6, 27),
                    EndDate = new DateTime(2026, 6, 27),
                    Coverage = 60,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 5, 26)
                });
        }
    }
}
