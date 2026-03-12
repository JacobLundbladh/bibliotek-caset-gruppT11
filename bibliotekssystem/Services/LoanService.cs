using bibliotekssystem.Models;

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

            var result = await _httpClient.GetFromJsonAsync<Loan[]>("loan");
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

            var result = await _httpClient.GetFromJsonAsync<Loan[]>("loan/{id}");
            return result ??  Array.Empty<Loan>(); // Ifall null skicka till backa tom array
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return Array.Empty<Loan>();
        }
    }
    
    // Skapa lån
    public async Task<Loan[]> CreateLoan(Loan loan)
    {
        try // Fel hantering
        {

            var result = await _httpClient.GetFromJsonAsync<Loan[]>("loan/{id}");
            return result ??  Array.Empty<Loan>(); // Ifall null skicka till backa tom array
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return Array.Empty<Loan>();
        }
    }

    
}