using System.Security.Claims;
using bibliotekssystem.Models;
using bibliotekssystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bibliotekssystem.Controllers;

[Authorize]
public class ReminderController : Controller
{
    private readonly ReminderService _reminderService;
    private readonly LoanService _loanService;
    private readonly AccountService _accountService;

    public ReminderController(
        ReminderService reminderService,
        LoanService loanService,
        AccountService accountService)
    {
        _reminderService = reminderService;
        _loanService = loanService;
        _accountService = accountService;
    }

    public async Task<IActionResult> Index()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Forbid();
        }

        int userId = int.Parse(userIdClaim);

        var userLoans = await _loanService.GetLoanByUser(userId);
        var allReminders = await _reminderService.GetRemindersAsync();

        var userLoanIds = userLoans.Select(l => l.Id).ToHashSet();

        var filteredReminders = allReminders
            .Where(r => userLoanIds.Contains(r.LoanId))
            .ToList();

        return View(filteredReminders);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> CreateReminder(int? selectedUserId)
    {
        var model = new CreateReminderViewModel
        {
            Users = (await _accountService.GetAccounts())
                .Where(a => a.Role != "Admin")
                .ToArray(),
            ReminderDate = DateTime.Now
        };

        if (selectedUserId.HasValue)
        {
            model.SelectedUserId = selectedUserId.Value;
            model.Loans = await _loanService.GetLoanByUser(selectedUserId.Value);
        }

        return View(model);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateReminder(CreateReminderViewModel model)
    {
        model.Users = (await _accountService.GetAccounts())
            .Where(a => a.Role != "Admin")
            .ToArray();

        if (model.SelectedUserId > 0)
        {
            model.Loans = await _loanService.GetLoanByUser(model.SelectedUserId);
        }

        if (model.SelectedUserId <= 0)
        {
            ModelState.AddModelError("", "Välj en användare.");
        }

        if (model.SelectedLoanId <= 0)
        {
            ModelState.AddModelError("", "Välj ett lån.");
        }

        if (string.IsNullOrWhiteSpace(model.Message))
        {
            ModelState.AddModelError("", "Skriv ett meddelande.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var reminder = new Reminder
        {
            LoanId = model.SelectedLoanId,
            Message = model.Message,
            ReminderDate = model.ReminderDate,
            IsSent = false
        };

        var success = await _reminderService.CreateReminderAsync(reminder);

        if (!success)
        {
            ViewBag.ErrorMessage = "Det gick inte att skicka påminnelsen.";
            return View(model);
        }

        TempData["SuccessMessage"] = "Påminnelsen skickades.";
        return RedirectToAction("Index");
    }
}