using System.ComponentModel.DataAnnotations;

namespace Library.ReminderAPI.Models
{
    public class Reminder
    {
        public int Id { get; set; }

        [Required]
        public int LoanId { get; set; }

        [Required]
        [StringLength(300)]
        public string Message { get; set; } = string.Empty;

        [Required]
        public DateTime ReminderDate { get; set; }

        public bool IsSent { get; set; } = false;
    }
}