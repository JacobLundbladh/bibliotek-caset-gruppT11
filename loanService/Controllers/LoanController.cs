using loanService.Data;
using Microsoft.AspNetCore.Mvc;

namespace loanService.Controllers;

[ApiController]
[Route("[controller]")]
public class LoanController : ControllerBase
{
    
    private readonly LoanServiceDbContext _dbContext;

    public LoanController(LoanServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public Loan[] GetLoans()
    {
        Loan[] loans = _dbContext.Loans.ToArray();
        return loans;
    }

    // Kanske lägga till så det skickas tillbaka svar
    [HttpPost]
    public IActionResult PostLoan(Loan loan)
    {
        _dbContext.Loans.Add(loan);
        _dbContext.SaveChanges();
        return Ok(loan);
    }
    
}