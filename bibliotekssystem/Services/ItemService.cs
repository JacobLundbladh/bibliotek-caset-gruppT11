namespace bibliotekssystem.Services;

using bibliotekssystem.Models;

public class ItemService
{
    private HttpClient _httpClient;

    public ItemService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Hämta alla objekt
    public async Task<Item[]> GetItems()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<Item[]>("Items");
            return result ?? Array.Empty<Item>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return Array.Empty<Item>();
        }
    }

    // Hämta ett objekt
    public async Task<Item?> GetItem(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Items/{id}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Kunde inte hämta objektet: {response.StatusCode}");
                return null;
            }

            var item = await _httpClient.GetFromJsonAsync<Item>($"Items/{id}");
            return item;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    // Skapa objekt
    public async Task<(bool Success, string ErrorMessage)> CreateItem(Item item)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("Items", item);

            if (response.IsSuccessStatusCode)
            {
                return (true, "");
            }

            var error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return (false, "Något gick fel vid kontakt med servern");
        }
    }

    // Ta bort objekt
    public async Task<bool> DeleteItem(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"Items/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    // Uppdatera objekt
    public async Task<bool> UpdateItem(Item item)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"Items/{item.Id}", item);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }
}