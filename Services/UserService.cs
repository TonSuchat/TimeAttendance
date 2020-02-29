using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeAttendance.Models;

namespace TimeAttendance.Services
{
    public class UserService
    {
        private readonly TimeAttendanceContext db;
        public UserService(TimeAttendanceContext db)
        {
            this.db = db;
        }
        public async Task<User> GetUserByEmail(string Email)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.Email == Email);
        }

        public async Task<User> GetUserById(int id)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> ChangePassword(int id, string newPassword)
        {
            if (id == 0 || string.IsNullOrEmpty(newPassword)) return false;
            try
            {
                var user = await GetUserById(id);
                if (user == null) return false;
                var salt = HashUtility.GenerateSalt();
                var hashed = HashUtility.HashPassword(newPassword, salt);
                user.Salt = salt;
                user.Password = hashed;
                db.Users.Update(user);
                var result = await db.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}