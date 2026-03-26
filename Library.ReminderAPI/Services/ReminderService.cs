using Library.ReminderAPI.Data;
using Library.ReminderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.ReminderAPI.Services
{
    public class ReminderService
    {
        private readonly LibraryContext _context;

        public ReminderService(LibraryContext context)
        {
            _context = context;
        }

        public async Task<List<Reminder>> GetAllAsync()
        {
            return await _context.Reminders.ToListAsync();
        }

        public async Task<Reminder?> GetByIdAsync(int id)
        {
            return await _context.Reminders.FindAsync(id);
        }

        public async Task<Reminder> CreateAsync(Reminder reminder)
        {
            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();
            return reminder;
        }

        public async Task<bool> UpdateAsync(Reminder reminder)
        {
            var existingReminder = await _context.Reminders.FindAsync(reminder.Id);

            if (existingReminder == null)
                return false;

            existingReminder.LoanId = reminder.LoanId;
            existingReminder.Message = reminder.Message;
            existingReminder.ReminderDate = reminder.ReminderDate;
            existingReminder.IsSent = reminder.IsSent;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reminder = await _context.Reminders.FindAsync(id);

            if (reminder == null)
                return false;

            _context.Reminders.Remove(reminder);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}