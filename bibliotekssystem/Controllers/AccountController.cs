using System.Security.Claims;

namespace bibliotekssystem.Controllers;
using bibliotekssystem.Models;    
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using bibliotekssystem.Services;

public class AccountController : Controller
{
    private AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }
    
    public IActionResult Index(string returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

   
    
    [HttpPost]
    public async Task<IActionResult> CreateAccount(Account account)
    {   
        if (string.IsNullOrEmpty(account.Username) || string.IsNullOrEmpty(account.Password) || string.IsNullOrEmpty(account.Role))
        {
            Console.WriteLine("Formuläret är inte komplett");
            return View("Create", account);
        }
        
        bool success = await _accountService.CreateAccount(account); // skicka till service/API

        if (success)
        {
            Console.WriteLine("Account created successfully");
                        
        }
        else
        {
            Console.WriteLine("Account creation failed");
             
        }
        return View("Index"); // 
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
    
    
    public async Task<IActionResult> Delete()
    {
        Account[] accounts = await _accountService.GetAccounts();
        return View(accounts);
    }


    public async Task<IActionResult> DeleteAccount(int id)
    {
        bool success = await _accountService.DeleteAccounts(id);

        if (success)
        {
            Console.WriteLine("Account deletd");
            return RedirectToAction("Delete");
        }
        else
        {
            Console.WriteLine("Deletion failed");
            return RedirectToAction("Delete");
        }
    }

}