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


    [HttpPost]
    public void PostLoan(Loan loan)
    {
        _dbContext.Loans.Add(loan);
        _dbContext.SaveChanges();
    }
}