using loanService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace loanService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanController : ControllerBase
{
    
    private readonly LoanServiceDbContext _dbContext;

    public LoanController(LoanServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<Loan[]> GetLoans()
    {
        return await _dbContext.Loans.ToArrayAsync();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoan(int id)
    {
        var loan = await _dbContext.Loans.FindAsync(id);
        return loan == null ? NotFound() : Ok(loan);  // vilkor, beroende, om sant, om falskt
    }

    [HttpPost]
    public async Task<IActionResult> PostLoan(Loan loan)
    {
        await _dbContext.Loans.AddAsync(loan); // Lägg till lån
        await _dbContext.SaveChangesAsync(); // Spara ändringar
        return Ok(loan); // Skicka tillbaka att det lyckades
    }

    [HttpDelete("{id}")] // HttpDelete för att ta bort
    public async Task<IActionResult> DeleteLoan(int id)
    {
        var loan = await _dbContext.Loans.FindAsync(id); // Hitta lån med det id

        if (loan == null)
        {
            return NotFound(); // Skicka tillbaka att den inte hittades
        }
        _dbContext.Loans.Remove(loan);
        await _dbContext.SaveChangesAsync();
        return Ok(loan);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLoan(int id, Loan loan)
    {
        if (id != loan.Id) // Kolla så id är korrekt
        {
            return BadRequest();
        }

        var existingLoan = await _dbContext.Loans.FindAsync(id);
        
        if (existingLoan == null) // Kolla så lån finns
        {
            return NotFound();
        }
        
        // Updatera fält
        existingLoan.UserId = loan.UserId;
        existingLoan.LoanDate = loan.LoanDate;
        existingLoan.DueDate = loan.DueDate;
        existingLoan.ReturnDate = loan.ReturnDate;
        existingLoan.Status = loan.Status;

        await _dbContext.SaveChangesAsync(); // Spara till databas
        
        return NoContent(); 

    }
}