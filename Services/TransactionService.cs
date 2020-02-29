using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeAttendance.Models;

namespace TimeAttendance.Services
{
    public class TransactionService
    {
        private readonly TimeAttendanceContext db;
        public TransactionService(TimeAttendanceContext db)
        {
            this.db = db;
        }

        public async Task<bool> AddTransaction(Transaction transaction)
        {
            if (transaction == null) return false;
            try
            {
                await db.Transactions.AddAsync(transaction);
                var result = await db.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Transaction> GetDashboard(int userId)
        {
            return db.Transactions.Where(t => t.UserId == userId &&
                    (t.Type == TransactionType.PUNCHIN || t.Type == TransactionType.PUNCHOUT));
        }
    }
}