using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TimeAttendance.Models
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
    }
}