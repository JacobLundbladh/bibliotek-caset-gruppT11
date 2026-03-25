namespace bibliotekssystem.Services;
using bibliotekssystem.Models;
using Microsoft.AspNetCore.Identity;

public class AccountService
{
    private HttpClient _httpClient;

    public AccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(bool Success, string ErrorMessage)> CreateAccount(Account account)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("User", account);

            if (response.IsSuccessStatusCode)
            {
                return (true, "");
            }

            // Läs fel från API
            var error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return (false, "Något gick fel vid kontakt med servern");
        }
    }
    
    
    // Hämta alla Konton
  
    public async Task<Account[]> GetAccounts()
    {
        try // Fel hantering
        {

            var result = await _httpClient.GetFromJsonAsync<Account[]>("User");
            return result ??  Array.Empty<Account>(); // Ifall null skicka till backa tom array
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return Array.Empty<Account>();
        }
    }

    public async Task<Account?> GetAccount(int id)
    {
        try
        {
            // Gör GET-anropet
            var response = await _httpClient.GetAsync($"User/{id}");

            // Kolla om det gick bra
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Kunde hämta användaren: {response.StatusCode}");
                return null;
            }

            
            var account = await _httpClient.GetFromJsonAsync<Account>($"User/{id}");

            return account;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
    
    public async Task<bool> DeleteAccounts(int id)
    {
        try // Fel hantering
        {

            var response = await _httpClient.DeleteAsync($"User/{id}");
            return response.IsSuccessStatusCode; // 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    public async Task<Account?> Login(Account account)
    {
       
        var response = await _httpClient.PostAsJsonAsync("User/login", account);
        Console.WriteLine($"Status: {response.StatusCode}"); // Debug
            
        var content = await response.Content.ReadAsStringAsync(); //Debug
        Console.WriteLine($"Response: {content}"); // Debug
            
        if (!response.IsSuccessStatusCode)
            return null;

            
        return await response.Content.ReadFromJsonAsync<Account>();
        
        
    }

    public async Task<bool> UpdateAccount(Account account)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"User/{account.Id}", account);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }
    
}