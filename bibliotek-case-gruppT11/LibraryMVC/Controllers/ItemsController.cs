using System.Net.Http.Json;
using LibraryMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Controllers;

public class ItemsController : Controller
{
    private readonly HttpClient _http = new HttpClient();

    public async Task<IActionResult> Index()
    {
        var items = await _http.GetFromJsonAsync<List<Item>>("https://localhost:XXXX/api/items");
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
        await _http.PostAsJsonAsync("https://localhost:XXXX/api/items", item);
        return RedirectToAction("Index");
    }
}