using Library.ItemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.ItemAPI.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }

    public DbSet<Item> Items { get; set; }
}