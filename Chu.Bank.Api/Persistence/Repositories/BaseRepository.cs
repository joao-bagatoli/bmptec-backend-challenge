using Chu.Bank.Api.Persistence.Contexts;

namespace Chu.Bank.Api.Persistence.Repositories;

public abstract class BaseRepository
{
    protected readonly AppDbContext _context;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }
}
