namespace loanService;

public class Loan
{
    public int Id { get; set; } // PK key
    public int ItemId { get; set; } // Hämta id från item tabellen
    public int UserId { get; set; } // Hämta id från user tabellen 
    public DateTime LoanDate { get; set; } // Datum för då lånet gjordes
    public DateTime DueDate { get; set; } // Datum för sista inlämnings datum
    public DateTime? ReturnDate { get; set; } // Datum då den lämnades tillbaka
    public string Status { get; set; } // Status på lånet 
    
}