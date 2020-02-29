using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAttendance.Models
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}