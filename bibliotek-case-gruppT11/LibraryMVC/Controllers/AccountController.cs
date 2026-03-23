using System.Net.Http.Json;
using System.Security.Claims;
using LibraryMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _http = new HttpClient();

    private const string UserApiBaseUrl = "http://localhost:XXXX/api/User";

    [HttpGet]
    public IActionResult Index(string returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(Account account, string returnUrl)
    {
        bool isAdmin = account.Username == "admin" && account.Password == "abc123";

        bool validUser = false;

        if (isAdmin)
        {
            validUser = true;
        }
        else
        {
            var users = await _http.GetFromJsonAsync<List<AppUser>>(UserApiBaseUrl) ?? new List<AppUser>();

            validUser = users.Any(u =>
                u.Username == account.Username &&
                u.Password == account.Password);
        }

        if (!validUser)
        {
            ViewBag.ErrorMessage = "Fel användarnamn eller lösenord";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(ClaimTypes.Name, account.Username));

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        if (!string.IsNullOrEmpty(returnUrl))
        {
            return Redirect(returnUrl);
        }

        if (account.Username == "admin")
        {
            return RedirectToAction("Index", "Admin");
        }

        return RedirectToAction("Index", "User");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(AppUser user)
    {
        await _http.PostAsJsonAsync(UserApiBaseUrl, user);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> SignOutUser()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}