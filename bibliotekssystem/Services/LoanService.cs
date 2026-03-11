using bibliotekssystem.Models;

namespace bibliotekssystem.Services;

public class LoanService
{
    private HttpClient _httpClient;

    public LoanService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Loan[]> GetLoan()
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
}