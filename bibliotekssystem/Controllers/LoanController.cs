using Microsoft.AspNetCore.Mvc;
using bibliotekssystem.Models;
using bibliotekssystem.Services;
using Microsoft.AspNetCore.Authorization;

namespace bibliotekssystem.Controllers;

public class LoanController : Controller
{
    private LoanService _loanService;
    private ItemService _itemService;
    public LoanController(LoanService loanService, ItemService itemService)
    {
        _loanService = loanService;
        _itemService = itemService;
    }
    
    public IActionResult Index()
    {
        //Loan[] loans = await _loanService.GetLoans();
        return View();
       
    }
    
    [Authorize]
    public async Task<IActionResult> Show()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;

        if (!int.TryParse(userIdClaim, out int userId))
            return Forbid();

        var loans = await _loanService.GetLoanByUser(userId);
        var items = await _itemService.GetItems();

        var viewModel = loans.Select(loan =>
        {
            var item = items.FirstOrDefault(i => i.Id == loan.ItemId);

            return new LoanViewModel
            {
                Id = loan.Id,
                UserId = loan.UserId,
                ItemTitle = item?.Title ?? "Okänd",
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate
            };
        }).ToArray();

        return View(viewModel);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        Item[] items = await _itemService.GetItems();
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

        var loans = await _loanService.GetLoanByUser(userId);
        var items = await _itemService.GetItems();

        var viewModel = loans.Select(loan =>
        {
            var item = items.FirstOrDefault(i => i.Id == loan.ItemId);

            return new LoanViewModel
            {
                Id = loan.Id,
                UserId = loan.UserId,
                ItemTitle = item?.Title ?? "Okänd",
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate
            };
        }).ToArray();

        return View(viewModel);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Finish(int id)
    {
        var item = await _itemService.GetItem(id);
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
        var item = await _itemService.GetItem(itemId);

        if (success)
        {
               
               item.IsAvailable = false; 
               await _itemService.UpdateItem(item);
               
               return RedirectToAction("MyLoans");
        }
                
         
        
        // Om inte lyckades skicka tillbaka item
        ViewBag.ErrorMessage = "Kunde inte skapa lånet";
        return View(item);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ReturnLoan(int id)
    {
        var loan = await _loanService.GetLoan(id);

        if (loan == null)
            return NotFound();

        loan.ReturnDate = DateTime.Now;
        loan.Status = "Återlämnad";

        bool success = await _loanService.UpdateLoan(loan);

        if (success)
        {
            var item = await _itemService.GetItem(loan.ItemId);

            if (item != null)
            {
                item.IsAvailable = true;
                await _itemService.UpdateItem(item);
            }
        }

        return RedirectToAction("MyLoans");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteLoan(int id)
    {
        bool success = await _loanService.DeleteLoan(id);

        if (!success)
        {
            TempData["ErrorMessage"] = "Kunde inte ta bort lånet.";
        }

        return RedirectToAction("Show");
    }
    
    
}