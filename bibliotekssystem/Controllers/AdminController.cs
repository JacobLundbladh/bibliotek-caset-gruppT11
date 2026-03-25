using Microsoft.AspNetCore.Authentication;

namespace bibliotekssystem.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

[Authorize]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> SignOutUser()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
    
}