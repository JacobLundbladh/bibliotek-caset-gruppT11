using System.Net.Http.Json;
using bibliotekssystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace bibliotekssystem.Controllers;

public class ItemController : Controller
{
    private readonly HttpClient _http;

    public ItemController(IHttpClientFactory httpClientFactory)
    {
        // Hämtar färdig HttpClient från Program.cs
        _http = httpClientFactory.CreateClient("ItemApi");
    }

    public async Task<IActionResult> Index()
    {
        // Hämtar alla items från ItemAPI
        var items = await _http.GetFromJsonAsync<List<Item>>("/api/Items");
        return View(items ?? new List<Item>());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Item item)
    {
        // Skickar nytt item till API
        await _http.PostAsJsonAsync("/api/Items", item);
        return RedirectToAction("Index");
    }
}