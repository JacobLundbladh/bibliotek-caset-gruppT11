using System.Security.Claims;

namespace bibliotekssystem.Controllers;
using bibliotekssystem.Models;    
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using bibliotekssystem.Services;
using Microsoft.AspNetCore.Authorization;

public class AccountController : Controller
{
    private AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }
    
    public IActionResult Index(string returnUrl)
    {
        // Om användaren redan är inloggad
        if (User.Identity?.IsAuthenticated ?? false)
        {
            // Gå direkt till Options (eller annan sida)
            return RedirectToAction("Options", "Account"); 
        }
        
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Options()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Update()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        Console.WriteLine($"CLAIM VALUE: {userIdClaim}");
        
        if (userIdClaim == null)
            return Forbid();

        int userId = int.Parse(userIdClaim);
        Console.WriteLine($"Claim ID: {userId}");
        
        var account = await _accountService.GetAccount(userId);
       
        if (account == null)
            return NotFound();

        return View(account); // Skicka med modell
    }
        
    [HttpPost]
    public async Task<IActionResult> CreateAccount(Account account)
    {   
        if (string.IsNullOrEmpty(account.Username) || string.IsNullOrEmpty(account.Password) || string.IsNullOrEmpty(account.Role))
        {
            Console.WriteLine("Formuläret är inte komplett");
            return View("Create", account);
        }
        
        var success = await _accountService.CreateAccount(account); // skicka till service/API

        if (success.Success)
        {
            Console.WriteLine("Account created successfully");
            return RedirectToAction("Index");
        }
        else
        {
            Console.WriteLine("Account creation failed");
            ViewBag.ErrorMessage = success.ErrorMessage;
            return View("Create", account);
        }
        
       
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Index(Account account, string returnUrl)
    {
        // KOlla inloggnings uppgifter
        var loggedInUser = await _accountService.Login(account);  
        
        // Fel 
        if (loggedInUser == null)
        {
            ViewBag.ErrorMessage = "Inloggning misslyckad: Fel användarnamn eller lösenord";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        
        // Korrekt
        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        
        //var user = await _accountService.GetAccount(account.Id);
        
        identity.AddClaim(new Claim(ClaimTypes.Name, loggedInUser.Username));
        identity.AddClaim(new Claim(ClaimTypes.Role, loggedInUser.Role ?? "Slutanvändare"));
        identity.AddClaim(new Claim("UserId", loggedInUser.Id.ToString()));
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

       
        
        // Ifall returnUrl inte finns, gå till home
        if (String.IsNullOrEmpty(returnUrl))
        {
            return RedirectToAction("Index", "Home");
        }
        
        // Gå tillbaka via returnUrl
        return Redirect(returnUrl);
    }
    
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete()
    {
        Account[] accounts = await _accountService.GetAccounts();
        return View(accounts);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        bool success = await _accountService.DeleteAccounts(id);
        
        // Hämta userId från cookie (claims)
        var userIdClaim = (User.FindFirst("UserId")?.Value);
        bool isSelf = false;
        
        if (userIdClaim != null && int.TryParse(userIdClaim, out int currentUserId))
        {
            isSelf = currentUserId == id;
        }
        
        if (success)
        {
            Console.WriteLine("Account deletd");
            if (isSelf)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Delete");
        }
        else
        {
            Console.WriteLine("Deletion failed");
            return RedirectToAction("Delete");
        }
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateAccount(Account account)
    {
        // Endast samma användare får redigera
        var userIdClaim = User.FindFirst("UserId")?.Value;
        if (userIdClaim != account.Id.ToString())
        {
            return Forbid(); // 403 om obehörig
        }
        
        var existingUser = await _accountService.GetAccount(account.Id);
        if (existingUser == null) // Här går det fel
        {
            ViewBag.ErrorMessage = $"Konto hittades inte";
            return View("Update");
        }

      
        
        // Sätt defaultvärden om användaren lämnar tomt
        if (string.IsNullOrWhiteSpace(account.Username))
            account.Username = existingUser.Username;

        if (string.IsNullOrWhiteSpace(account.Password))
            account.Password = existingUser.Password; // redan hashat
        
        
        if (string.IsNullOrWhiteSpace(account.Role))
            account.Role = existingUser.Role;
        
        bool success = await _accountService.UpdateAccount(account);
        if (success)
        {
            // Skapa ny ClaimsIdentity med uppdaterade värden
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, account.Username));
            identity.AddClaim(new Claim(ClaimTypes.Role, account.Role ?? "Slutanvändare"));
            identity.AddClaim(new Claim("UserId", account.Id.ToString()));

            // Signa in igen för att uppdatera cookie
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
            
            return RedirectToAction("Options"); // kontosida
        }
            
        else
        {
            // Skicka felmeddelande till vyn
            ViewBag.ErrorMessage = "Uppdateringen misslyckades. Kontrollera att alla fält är korrekt ifyllda.";
            return View("Update"); // visa formuläret igen vid fel
        }
            

         
    }
    
    
    public IActionResult AccessDenied(string returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }
    
    
    
    public async Task<IActionResult> SignOutUser()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}