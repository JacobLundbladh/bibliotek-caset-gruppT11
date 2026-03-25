namespace bibliotekssystem.Models;

public class Loan
{
    public int Id { get; set; } // PK key
    public required int ItemId { get; set; } // Hämta id från item tabellen
    public required int UserId { get; set; } // Hämta id från user tabellen 
    public required DateTime LoanDate { get; set; } // Datum för då lånet gjordes
    public required DateTime DueDate { get; set; } // Datum för sista inlämnings datum
    public DateTime? ReturnDate { get; set; } // Datum då den lämnades tillbaka
    
    public required string Status { get; set; } // Status på lånet 
    
}