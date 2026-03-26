using System.Net.Http.Json;
using bibliotekssystem.Models;

namespace bibliotekssystem.Services
{
    public class ReminderService
    {
        private readonly HttpClient _httpClient;

        public ReminderService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ReminderService");
        }

        public async Task<List<Reminder>> GetRemindersAsync()
        {
            var reminders = await _httpClient.GetFromJsonAsync<List<Reminder>>("Reminders");
            return reminders ?? new List<Reminder>();
        }
    }
}