using System.Net.Http.Json;
using bibliotekssystem.Models;
using Microsoft.AspNetCore.Mvc;
using bibliotekssystem.Services;

namespace bibliotekssystem.Controllers;

public class ItemController : Controller
{
    private ItemService _itemService;

    public ItemController(ItemService itemService)
    {
        // Hämtar färdig HttpClient från Program.cs
        _itemService = itemService;
    }

    public async Task<IActionResult> Index()
    {
        // Hämtar alla items från ItemAPI
        var items = await _itemService.GetItems();
        return View(items?.ToList() ?? new List<Item>());
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
        await _itemService.CreateItem(item);
        return RedirectToAction("Index");
    }
}