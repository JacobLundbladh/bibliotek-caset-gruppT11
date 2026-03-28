namespace bibliotekssystem.Models
{
    public class CreateReminderViewModel
    {
        public int SelectedUserId { get; set; }
        public int SelectedLoanId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime ReminderDate { get; set; } = DateTime.Now;

        public Account[] Users { get; set; } = Array.Empty<Account>();
        public Loan[] Loans { get; set; } = Array.Empty<Loan>();
    }
}