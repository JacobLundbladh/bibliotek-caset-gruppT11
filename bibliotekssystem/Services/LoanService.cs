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
    
    public async Task<Loan[]> GetLoan(int id)
    {
        try // Fel hantering
        {

            var result = await _httpClient.GetFromJsonAsync<Loan[]>($"loan/{id}");
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
        // Hårdkoda fält som vi inte fyller i formuläret
        //loan.UserId = 42; // exempel på test-användare
        loan.LoanDate = DateTime.Now;
        loan.DueDate = DateTime.Now.AddDays(14); // 2 veckors lån
        loan.ReturnDate = null;
        loan.Status = "Active";

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

    public async Task<Item[]> GetItems()
    {
        
        try // Fel hantering
        {

            var result = await _httpClient.GetFromJsonAsync<Item[]>("items"); // Stor/liten
            return result ??  Array.Empty<Item>(); // Ifall null skicka till backa tom array
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return Array.Empty<Item>();
        }
    }

    
}