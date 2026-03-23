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
    
    
    public async Task<bool> CreateAccount(Account account)
    {
       
        try // Fel hantering
        {
            // Hash
            var hasher = new PasswordHasher<Account>();
            account.Password = hasher.HashPassword(null, account.Password);

            var response = await _httpClient.PostAsJsonAsync("User", account);
            Console.WriteLine(response.StatusCode);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
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

    public async Task<bool> DeleteAccounts(int id)
    {
        try // Fel hantering
        {

            var result = await _httpClient.DeleteAsync($"User/{id}");
            return true; // Ifall null skicka till backa tom array
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }
    
}