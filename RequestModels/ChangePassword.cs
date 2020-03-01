using System.ComponentModel.DataAnnotations;

namespace TimeAttendance.RequestModels
{
    public class ChangePassword
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}