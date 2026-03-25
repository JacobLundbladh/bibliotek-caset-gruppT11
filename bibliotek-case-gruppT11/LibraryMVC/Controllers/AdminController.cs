using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Controllers;

[Authorize]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        if (User.Identity?.Name != "admin")
        {
            return RedirectToAction("Index", "User");
        }

        return View();
    }
}