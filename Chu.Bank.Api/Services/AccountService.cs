using Chu.Bank.Api.Domain.Models;
using Chu.Bank.Api.Domain.Repositories;
using Chu.Bank.Api.Domain.Services;
using static Chu.Bank.Api.Domain.DTOs.AccountDtos;

namespace Chu.Bank.Api.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AccountService(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AccountResponseDto> GetByIdAsync(Guid id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if (account == null)
            throw new InvalidOperationException($"Account {id} not found");
        return ToResponseDto(account);
    }

    public async Task<AccountResponseDto> CreateAsync(AccountRequestDto request, CancellationToken cancellationToken)
    {
        var account = new Account(request.Holder, request.Balance);
        _accountRepository.Add(account);
        await _unitOfWork.CommitAsync(cancellationToken);
        return ToResponseDto(account);
    }

    private static AccountResponseDto ToResponseDto(Account account)
    {
        return new AccountResponseDto(account.Id, account.Holder, account.Balance);
    }
}
