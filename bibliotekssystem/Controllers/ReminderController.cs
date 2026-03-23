namespace bibliotekssystem.Controllers;
using Microsoft.AspNetCore.Mvc;

public class ReminderController : Controller
{

    public IActionResult Index()
    {
        return View();
    }
}