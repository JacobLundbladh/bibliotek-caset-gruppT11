using Microsoft.AspNetCore.Mvc;
using bibliotekssystem.Models;
using bibliotekssystem.Services;
using Microsoft.AspNetCore.Authorization;

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
    
    [Authorize]
    public async Task<IActionResult> Show()
    {
        Loan[] loans = await _loanService.GetLoans();
        return View(loans);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        Item[] items = await _loanService.GetItems();
        return View(items);
    }
    [Authorize]
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
    
    [Authorize]
    public async Task<IActionResult> MyLoans()
    {
        
        var userIdClaim = User.FindFirst("UserId")?.Value;

        if (!int.TryParse(userIdClaim, out int userId))
            return Forbid();
        
        Loan[] loans = await _loanService.GetLoanByUser(userId);
        
        return View(loans);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Finish(int id)
    {
        var item = await _loanService.GetItem(id);
        if (item == null)
            return NotFound();
        
        return View(item); // item ska skickas
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Finish(int itemId, DateTime dueDate)
    {
        var userIdClaim = User.FindFirst("UserId")?.Value; // Hämta id för användaren

        if (!int.TryParse(userIdClaim, out int userId))
            return Forbid();

        var loan = new Loan // Sätt värden på lånet
        {
            ItemId = itemId,
            UserId = userId,
            LoanDate = DateTime.Now,
            DueDate = dueDate,
            Status = "Aktiv"
        };

        bool success = await _loanService.CreateLoan(loan);

        if (success)
            return RedirectToAction("Show");
        
        // Om inte lyckades skicka tillbaka item
        var item = await _loanService.GetItem(itemId);
        ViewBag.ErrorMessage = "Kunde inte skapa lånet";
        return View(item);
    }
    
}