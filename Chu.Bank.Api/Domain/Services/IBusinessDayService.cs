namespace Chu.Bank.Api.Domain.Services;

public interface IBusinessDayService
{
    Task<bool> IsBusinessDayAsync(DateTime date);
}
