using Library.ItemAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.ItemAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private static List<Item> items = new List<Item>();

    [HttpGet]
    public ActionResult<IEnumerable<Item>> GetItems()
    {
        return items;
    }

    [HttpGet("{id}")]
    public ActionResult<Item> GetItem(int id)
    {
        var item = items.FirstOrDefault(i => i.Id == id);

        if (item == null)
            return NotFound();

        return item;
    }

    [HttpPost]
    public ActionResult<Item> CreateItem(Item item)
    {
        items.Add(item);
        return item;
    }

    [HttpPut("{id}")]
    public IActionResult UpdateItem(int id, Item updatedItem)
    {
        var item = items.FirstOrDefault(i => i.Id == id);

        if (item == null)
            return NotFound();

        item.Title = updatedItem.Title;
        item.Category = updatedItem.Category;
        item.Description = updatedItem.Description;
        item.IsAvailable = updatedItem.IsAvailable;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteItem(int id)
    {
        var item = items.FirstOrDefault(i => i.Id == id);

        if (item == null)
            return NotFound();

        items.Remove(item);

        return NoContent();
    }
}