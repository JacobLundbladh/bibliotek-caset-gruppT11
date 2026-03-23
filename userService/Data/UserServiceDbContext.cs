using Microsoft.EntityFrameworkCore;


namespace userService.Data;

public class UserServiceDbContext : DbContext
{
    public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options)
        : base(options) { }
    
    public DbSet<User> Users { get; set; }
}