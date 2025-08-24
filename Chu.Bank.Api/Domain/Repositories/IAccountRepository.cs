using Chu.Bank.Api.Domain.Models;

namespace Chu.Bank.Api.Domain.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id);
    void Add(Account account);
    void Update(Account account);
}
