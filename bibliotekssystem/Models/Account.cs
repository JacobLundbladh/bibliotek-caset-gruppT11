namespace bibliotekssystem.Models;
 // Samma som User
public class Account
{
    public int Id { get; set; } // PK key
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string? Role { get; set; }
}