using System.ComponentModel.DataAnnotations;

namespace TimeAttendance.RequestModels
{
    public class LogIn
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}