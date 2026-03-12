using Microsoft.AspNetCore.Mvc;
using bibliotekssystem.Models;
using bibliotekssystem.Services;

namespace bibliotekssystem.Controllers;

public class LoanController : Controller
{
    private LoanService _loanService;
    
    public LoanController(LoanService loanService)
    {
        _loanService = loanService;
    }
    
    public async Task<IActionResult> Index()
    {
        Loan[] loans = await _loanService.GetLoans();
        return View(loans);
    }
}