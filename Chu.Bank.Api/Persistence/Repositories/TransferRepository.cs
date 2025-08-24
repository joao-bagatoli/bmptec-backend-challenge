using Chu.Bank.Api.Domain.Models;
using Chu.Bank.Api.Domain.Repositories;
using Chu.Bank.Api.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Chu.Bank.Api.Persistence.Repositories
{
    public class TransferRepository : BaseRepository, ITransferRepository
    {
        public TransferRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Transfer?> GetByIdAsync(Guid id)
        {
            return await _context.Transfers.FindAsync(id);
        }

        public void Add(Transfer transfer)
        {
            _context.Transfers.Add(transfer);
        }

        public async Task<List<Transfer>> GetByPeriodAsync(Guid accountId, DateTime startDate, DateTime endDate)
        {
            return await _context.Transfers
                .AsNoTracking()
                .Where(t => 
                    (t.FromAccountId == accountId || t.ToAccountId == accountId) &&
                    t.TransferredAt >= startDate && t.TransferredAt <= endDate)
                .ToListAsync();
        }
    }
}
