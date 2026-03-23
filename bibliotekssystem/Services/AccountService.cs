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
    
    
}