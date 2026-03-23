using System.Net.Http.Json;
using LibraryMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Controllers;

[Authorize]
public class ItemsController : Controller
{
    private readonly HttpClient _http = new HttpClient();

    public async Task<IActionResult> Index()
    {
        var items = await _http.GetFromJsonAsync<List<Item>>("http://localhost:5265/api/Items");
        return View(items ?? new List<Item>());
    }

    [HttpGet]
    public IActionResult Create()
    {
        if (User.Identity?.Name != "admin")
        {
            return RedirectToAction("Index");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Item item)
    {
        if (User.Identity?.Name != "admin")
        {
            return RedirectToAction("Index");
        }

        await _http.PostAsJsonAsync("http://localhost:5265/api/Items", item);
        return RedirectToAction("Index");
    }
}