using Microsoft.AspNetCore.Mvc;

namespace loanService.Controllers;

[ApiController]
[Route("[controller]")]
public class LoanController : ControllerBase
{
    

    [HttpGet(Name = "GetLoan")]
    public IEnumerable<Loan> Get()
    {
        return new List<Loan>
        {
            new Loan { ID = 1, ItemID = 5, UserID = 0, LoanDate = DateTime.Now.AddDays(-14), DueDate = DateTime.Now.AddDays(-7), ReturnDate = DateTime.Now, Status = "Sen inlämning" },
            new Loan { ID = 2, ItemID = 7, UserID = 0, LoanDate = DateTime.Now, DueDate = DateTime.Now.AddDays(7), ReturnDate = null, Status = "Lånad" }           
        };
    }
}