using System.ComponentModel.DataAnnotations;

namespace TimeAttendance.RequestModels
{
    public class LogOut
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Status { get; set; }
    }
}