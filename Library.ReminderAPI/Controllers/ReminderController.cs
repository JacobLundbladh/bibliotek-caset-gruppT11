using Library.ReminderAPI.Models;
using Library.ReminderAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.ReminderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemindersController : ControllerBase
    {
        private readonly ReminderService _reminderService;

        public RemindersController(ReminderService reminderService)
        {
            _reminderService = reminderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reminder>>> GetReminders()
        {
            var reminders = await _reminderService.GetAllAsync();
            return Ok(reminders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reminder>> GetReminder(int id)
        {
            var reminder = await _reminderService.GetByIdAsync(id);

            if (reminder == null)
                return NotFound();

            return Ok(reminder);
        }

        [HttpPost]
        public async Task<ActionResult<Reminder>> PostReminder(Reminder reminder)
        {
            var createdReminder = await _reminderService.CreateAsync(reminder);

            return CreatedAtAction(nameof(GetReminder), new { id = createdReminder.Id }, createdReminder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutReminder(int id, Reminder reminder)
        {
            if (id != reminder.Id)
                return BadRequest();

            var updated = await _reminderService.UpdateAsync(reminder);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReminder(int id)
        {
            var deleted = await _reminderService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}