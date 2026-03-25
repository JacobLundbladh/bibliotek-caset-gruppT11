using System.Net.Http.Json;
using bibliotekssystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace bibliotekssystem.Controllers;

public class ItemController : Controller
{
    private readonly HttpClient _http = new HttpClient();

    private readonly string baseUrl = "https://app-sos100-itemapi-dreni-h7hfczh6g8fxb8gv.norwayeast-01.azurewebsites.net";

    public async Task<IActionResult> Index()
    {
        var items = await _http.GetFromJsonAsync<List<Item>>($"{baseUrl}/api/Items");
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
        await _http.PostAsJsonAsync($"{baseUrl}/api/Items", item);
        return RedirectToAction("Index");
    }
}