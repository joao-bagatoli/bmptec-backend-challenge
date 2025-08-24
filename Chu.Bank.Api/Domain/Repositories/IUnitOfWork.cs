namespace Chu.Bank.Api.Domain.Repositories;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken);
}
