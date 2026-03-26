namespace bibliotekssystem.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime ReminderDate { get; set; }
        public bool IsSent { get; set; }
    }
}