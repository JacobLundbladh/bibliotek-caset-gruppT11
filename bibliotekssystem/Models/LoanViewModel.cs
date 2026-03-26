namespace bibliotekssystem.Models;

public class LoanViewModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ItemTitle { get; set; } = "";
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}