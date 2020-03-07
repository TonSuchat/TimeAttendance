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

        public IEnumerable<Dashboard> GetDashboard(int userId)
        {
            var transactions = db.Transactions.Where(t => t.UserId == userId &&
                    (t.Type == TransactionType.PUNCHIN || t.Type == TransactionType.PUNCHOUT))
                    .OrderBy(t => t.CreatedDate).ToList();
            if (transactions == null) return null;
            var dashboards = transactions.GroupBy(t => t.CreatedDate.ToString("yyyy-MM-dd"))
                       .Select(t => new Dashboard()
                       {
                           Id = userId,
                           StampTime = t.Key,
                           StampTimeIn = t.Where(x => x.Type == TransactionType.PUNCHIN)
                                          .Min(x => x.CreatedDate).ToString("HH:mm") ?? "",
                           StampTimeOut = t.Where(x => x.Type == TransactionType.PUNCHOUT)
                                          .Max(x => x.CreatedDate).ToString("HH:mm") ?? ""
                       }
            );
            return dashboards.ToList();
        }
    }
}