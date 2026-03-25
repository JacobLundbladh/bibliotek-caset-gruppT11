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
    
    public IActionResult Index()
    {
        //Loan[] loans = await _loanService.GetLoans();
        return View();
       
    }

    public async Task<IActionResult> Show()
    {
        Loan[] loans = await _loanService.GetLoans();
        return View(loans);
    }
    
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        Item[] items = await _loanService.GetItems();
        return View(items);
    }
    [HttpPost]
    public async Task<IActionResult> Create(Loan loan) // Skapa lån objekt
    {
        // Hårdkoda fält för test
        //loan.UserId = 42;
        //loan.LoanDate = DateTime.Now;
        //loan.DueDate = DateTime.Now.AddDays(14);
        //loan.ReturnDate = null;
        //loan.Status = "Active";
    
        bool success = await _loanService.CreateLoan(loan); // skicka till service/API
        
        if (success)
            return RedirectToAction("Show"); // gå till lista av lån
        else
            return View(loan); // visa formuläret igen vid fel
    }

    public async Task<IActionResult> MyLoans()
    {
        
        var userIdClaim = User.FindFirst("UserId")?.Value;

        if (!int.TryParse(userIdClaim, out int userId))
            return Forbid();
        
        Loan[] loans = await _loanService.GetLoanByUser(userId);
        
        return View(loans);
    }

    [HttpGet]
    public async Task<IActionResult> Finish(int id)
    {
        var item = await _loanService.GetItem(id);
        if (item == null)
            return NotFound();
        
        return View(); // item ska skickas
    }
}