using Chu.Bank.Api.Domain.Models;
using Chu.Bank.Api.Domain.Repositories;
using Chu.Bank.Api.Persistence.Contexts;

namespace Chu.Bank.Api.Persistence.Repositories;

public class AccountRepository : BaseRepository, IAccountRepository
{
    public AccountRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        return await _context.Accounts.FindAsync(id);
    }

    public void Add(Account account)
    {
        _context.Accounts.Add(account);
    }

    public void Update(Account account)
    {
        _context.Accounts.Update(account);
    }
}
