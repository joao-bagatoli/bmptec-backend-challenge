using Chu.Bank.Api.Domain.Repositories;
using Chu.Bank.Api.Persistence.Contexts;

namespace Chu.Bank.Api.Persistence.Repositories;

public class UnitOfWork : BaseRepository, IUnitOfWork
{
    public UnitOfWork(AppDbContext context) : base(context)
    {
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
