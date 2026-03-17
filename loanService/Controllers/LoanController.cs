using loanService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<Loan[]> GetLoans()
    {
        return await _dbContext.Loans.ToArrayAsync();
    }

    [HttpPost]
    public async Task<IActionResult> PostLoan(Loan loan)
    {
        await _dbContext.Loans.AddAsync(loan);
        await _dbContext.SaveChangesAsync();
        return Ok(loan);
    }
    
}