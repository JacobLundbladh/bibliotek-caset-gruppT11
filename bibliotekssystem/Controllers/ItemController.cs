namespace bibliotekssystem.Controllers;
using Microsoft.AspNetCore.Mvc;

public class ItemController : Controller
{

    public IActionResult Index()
    {
        return View();
    }
}
