using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        private IEnumerable<Dashboard> CreateDashboardByTransaction(IList<Transaction> transactions, int userId)
        {
            var transactionGroup = transactions.GroupBy(t => t.CreatedDate.ToString("yyyy-MM-dd"));
            var dashboard = new List<Dashboard>();
            foreach (var item in transactionGroup)
            {
                var newDashboard = new Dashboard() { Id = userId, StampTime = item.Key };
                if (item.Any(x => x.Type == TransactionType.PUNCHIN))
                {
                    newDashboard.StampTimeIn = item.Where(x => x.Type == TransactionType.PUNCHIN)
                                               .Min(x => x.CreatedDate).ToString("HH:mm");
                }
                if (item.Any(x => x.Type == TransactionType.PUNCHOUT))
                {
                    newDashboard.StampTimeOut = item.Where(x => x.Type == TransactionType.PUNCHOUT)
                                                .Max(x => x.CreatedDate).ToString("HH:mm");
                }
                dashboard.Add(newDashboard);
            }
            return dashboard;
        }

        public async Task<IEnumerable<Dashboard>> GetDashboards(int userId)
        {
            var transactions = await db.Transactions.Where(t => t.UserId == userId &&
                    (t.Type == TransactionType.PUNCHIN || t.Type == TransactionType.PUNCHOUT))
                    .OrderBy(t => t.CreatedDate).ToListAsync();
            if (transactions == null) return null;
            return CreateDashboardByTransaction(transactions, userId);
        }

        public async Task<Dashboard> GetDashboard(int userId, DateTime dateTime)
        {
            var transactions = await db.Transactions.Where(t => t.UserId == userId &&
                    (t.Type == TransactionType.PUNCHIN || t.Type == TransactionType.PUNCHOUT) &&
                    t.CreatedDate.Date == dateTime.Date)
                    .OrderBy(t => t.CreatedDate).ToListAsync();
            if (transactions == null) return null;
            return CreateDashboardByTransaction(transactions, userId).FirstOrDefault();
        }
    }
}