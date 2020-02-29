using Microsoft.EntityFrameworkCore;

namespace TimeAttendance.Models
{
    public class TimeAttendanceContext : DbContext
    {
        public TimeAttendanceContext(DbContextOptions<TimeAttendanceContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}