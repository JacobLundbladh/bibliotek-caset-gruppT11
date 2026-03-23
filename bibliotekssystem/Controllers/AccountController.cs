using System.Security.Claims;

namespace bibliotekssystem.Controllers;
using bibliotekssystem.Models;    
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

public class AccountController : Controller
{
    public IActionResult Index(string returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(Account account, string returnUrl)
    {
        // KOlla inloggnings uppgifter
        bool accountValid = account.Username == "admin" && account.Password == "admin";  
        
        // Fel 
        if (!accountValid)
        {
            ViewBag.ErrorMessage = "Inloggning misslyckad: Fel användarnamn eller lösenord";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        
        // Korrekt
        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(ClaimTypes.Name, account.Username));
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        
        // Ifall returnUrl inte finns, gå till home
        if (String.IsNullOrEmpty(returnUrl))
        {
            return RedirectToAction("Index", "Home");
        }
        
        // Gå tillbaka via returnUrl
        return Redirect(returnUrl);
    }
}