using System.Net.Http.Json;
using bibliotekssystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace bibliotekssystem.Controllers;

public class ItemController : Controller
{
    private readonly HttpClient _http = new HttpClient();

    // URL till ditt ItemAPI i Azure
    private readonly string baseUrl = "https://app-sos100-itemapi-dreni-h7hfczh6g8fxb8gv.norwayeast-01.azurewebsites.net";

    // API-nyckeln hämtas från konfiguration (Azure Environment Variable)
    private readonly string _apiKey;

    public ItemController(IConfiguration config)
    {
        // Hämtar nyckeln som vi la in i Azure (ItemApiKey)
        _apiKey = config["ItemApiKey"]!;

        // Lägger till API-nyckeln i headern för alla anrop
        _http.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
    }

    public async Task<IActionResult> Index()
    {
        // Hämtar alla items från ItemAPI
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
        // Skickar nytt item till API (med API-nyckel automatiskt i headern)
        await _http.PostAsJsonAsync($"{baseUrl}/api/Items", item);
        return RedirectToAction("Index");
    }
}