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
    }
}