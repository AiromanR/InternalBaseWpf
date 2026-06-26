using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using InternalBaseWpf.Data;
using InternalBaseWpf.Models;

namespace InternalBaseWpf.Service
{
    public class UserService
    {
        private readonly AppDbContext _db = DBService.Instance.Context;

        public User? Authenticate(string login, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Login == login && u.IsActive);
            if (user == null)
                return null;

            return PasswordService.VerifyPassword(password, user.PasswordHash, user.Salt) ? user : null;
        }

        public List<User> GetAll()
        {
            return _db.Users.OrderBy(u => u.FullName).ToList();
        }

        public User? GetById(int id)
        {
            return _db.Users.Find(id);
        }

        public string ResetPassword(int userId)
        {
            var user = _db.Users.Find(userId);
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден");

            string newPassword = PasswordService.GeneratePassword();
            var (hash, salt) = PasswordService.HashPassword(newPassword);
            user.PasswordHash = hash;
            user.Salt = salt;
            _db.SaveChanges();
            return newPassword;
        }

        public User Create(string login, string fullName, bool isAdmin, out string generatedPassword)
        {
            if (_db.Users.Any(u => u.Login == login))
                throw new InvalidOperationException("Пользователь с таким логином уже существует");

            generatedPassword = PasswordService.GeneratePassword();
            var (hash, salt) = PasswordService.HashPassword(generatedPassword);

            var user = new User
            {
                Login = login,
                FullName = fullName,
                PasswordHash = hash,
                Salt = salt,
                IsAdmin = isAdmin,
                IsActive = true
            };

            _db.Users.Add(user);
            _db.SaveChanges();
            return user;
        }

        public void ToggleAdmin(int userId)
        {
            var user = _db.Users.Find(userId);
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден");

            user.IsAdmin = !user.IsAdmin;
            _db.SaveChanges();
        }

        public void Delete(int userId)
        {
            var user = _db.Users.Find(userId);
            if (user == null)
                return;

            _db.Users.Remove(user);
            _db.SaveChanges();
        }
    }
}
