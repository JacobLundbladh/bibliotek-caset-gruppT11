using bibliotekssystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace bibliotekssystem.Controllers;

public class ReminderController : Controller
{
    private readonly ReminderService _reminderService;

    public ReminderController(ReminderService reminderService)
    {
        _reminderService = reminderService;
    }

    public async Task<IActionResult> Index()
    {
        var reminders = await _reminderService.GetRemindersAsync();
        return View(reminders);
    }
}