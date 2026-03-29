using bibliotekssystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace bibliotekssystem.Services;

public class LoanService
{
    private HttpClient _httpClient;

    public LoanService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    // Hämta alla lån
  
    public async Task<Loan[]> GetLoans()
    {
        try // Fel hantering
        {

            var result = await _httpClient.GetFromJsonAsync<Loan[]>("Loan"); // Stor/liten
            return result ??  Array.Empty<Loan>(); // Ifall null skicka till backa tom array
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return Array.Empty<Loan>();
        }
    }
    
    // Hämta beroende på id
    
    public async Task<Loan> GetLoan(int id)
    {
        try // Fel hantering
        {

            var result = await _httpClient.GetFromJsonAsync<Loan>($"loan/{id}");
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
    
    public async Task<Loan[]> GetLoanByUser(int userId)
    {
        try // Fel hantering
        {

            var result = await _httpClient.GetFromJsonAsync<Loan[]>($"loan/user/{userId}");
            return result ??  Array.Empty<Loan>(); // Ifall null skicka till backa tom array
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return Array.Empty<Loan>();
        }
    }

    
    // Skapa lån
 
    public async Task<bool> CreateLoan(Loan loan)
    {
        // Hårdkoda fält som inte fyller i formuläret
        
        loan.ReturnDate = null;
        

        try // Fel hantering
        {
            

            var response = await _httpClient.PostAsJsonAsync("loan", loan);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    
    public async Task<Item?> GetItem(int id)
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<Item>($"items/{id}");
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
    
    
    
    public async Task<bool> UpdateLoan(Loan loan)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"Loan/{loan.Id}", loan);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> DeleteLoan(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"Loan/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }
}