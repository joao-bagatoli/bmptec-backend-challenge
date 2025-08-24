using static Chu.Bank.Api.Domain.DTOs.AccountDtos;

namespace Chu.Bank.Api.Domain.Services;

public interface IAccountService
{
    Task<AccountResponseDto> GetByIdAsync(Guid id);
    Task<AccountResponseDto> CreateAsync(AccountRequestDto request, CancellationToken cancellationToken);
}
