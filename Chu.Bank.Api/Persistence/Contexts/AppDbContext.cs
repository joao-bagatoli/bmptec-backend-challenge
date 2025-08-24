using Chu.Bank.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Chu.Bank.Api.Persistence.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
}
