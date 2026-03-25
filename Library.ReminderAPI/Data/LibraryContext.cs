using Library.ReminderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.ReminderAPI.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        public DbSet<Reminder> Reminders { get; set; }
    }
}