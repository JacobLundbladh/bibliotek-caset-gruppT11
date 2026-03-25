using System.Net.Http.Json;

namespace bibliotekssystem.Services
{
    public class ReminderService
    {
        private readonly HttpClient _httpClient;

        public ReminderService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ReminderService");
        }

        public async Task<string> GetRemindersRawAsync()
        {
            var response = await _httpClient.GetAsync("Reminders");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}