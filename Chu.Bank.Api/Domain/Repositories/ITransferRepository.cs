using Chu.Bank.Api.Domain.Models;

namespace Chu.Bank.Api.Domain.Repositories;

public interface ITransferRepository
{
    Task<Transfer?> GetByIdAsync(Guid id);
    void Add(Transfer transfer);
    Task<List<Transfer>> GetByPeriodAsync(Guid accountId, DateTime startDate, DateTime endDate);
}
