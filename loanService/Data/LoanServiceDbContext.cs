using Microsoft.EntityFrameworkCore;

namespace loanService.Data;

public class LoanServiceDbContext : DbContext
{
    public LoanServiceDbContext(DbContextOptions<LoanServiceDbContext> options)
        : base(options)
    {
        
    }
    
    public DbSet<Loan> Loans { get; set; }
    
}