using System.Security.Claims;
using bibliotekssystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bibliotekssystem.Controllers;

[Authorize]
public class ReminderController : Controller
{
    private readonly ReminderService _reminderService;
    private readonly LoanService _loanService;

    public ReminderController(ReminderService reminderService, LoanService loanService)
    {
        _reminderService = reminderService;
        _loanService = loanService;
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
}