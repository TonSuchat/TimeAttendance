using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAttendance.Models
{
    [Table("Transaction")]
    public class Transaction
    {
        public Transaction()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public TransactionType Type { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}