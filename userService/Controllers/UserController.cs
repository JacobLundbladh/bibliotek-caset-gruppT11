using userService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using userService;
namespace userService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    
    private readonly UserServiceDbContext _dbContext;

    public UserController(UserServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /*
    [HttpGet]
    public User[] GetUser()
    {
        User[] users = _dbContext.Users.ToArray();
        return users;
    }

    [HttpPost]
    public void PostUser(User user)
    {
        
    }
    */
    
    
    [HttpGet]
    public async Task<User[]> GetUsers()
    {
        return await _dbContext.Users.ToArrayAsync();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        return user == null ? NotFound() : Ok(user);  // vilkor, beroende, om sant, om falskt
    }

    [HttpPost]
    public async Task<IActionResult> PostUser(User user)
    {
        await _dbContext.Users.AddAsync(user); // Lägg till lån
        await _dbContext.SaveChangesAsync(); // Spara ändringar
        return Ok(user); // Skicka tillbaka att det lyckades
    }

    [HttpDelete("{id}")] // HttpDelete för att ta bort
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _dbContext.Users.FindAsync(id); // Hitta lån med det id

        if (user == null)
        {
            return NotFound(); // Skicka tillbaka att den inte hittades
        }
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        if (id != user.Id) // Kolla så id är korrekt
        {
            return BadRequest();
        }

        var existingUser = await _dbContext.Users.FindAsync(id);
        
        if (existingUser == null) // Kolla så lån finns
        {
            return NotFound();
        }
        
        // Updatera fält
        

        await _dbContext.SaveChangesAsync(); // Spara till databas
        
        return NoContent(); 

    } 
}