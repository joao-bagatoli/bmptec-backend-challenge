using Chu.Bank.Api.Domain.Models;
using Chu.Bank.Api.Domain.Repositories;
using Chu.Bank.Api.Domain.Services;
using static Chu.Bank.Api.Domain.DTOs.TransferDtos;

namespace Chu.Bank.Api.Services;

public class TransferService : ITransferService
{
    private readonly ITransferRepository _transferRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBusinessDayService _businessDayService;

    public TransferService(ITransferRepository transferRepository, IAccountRepository accountRepository, IUnitOfWork unitOfWork, IBusinessDayService businessDayService)
    {
        _transferRepository = transferRepository;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _businessDayService = businessDayService;
    }

    public async Task<TransferResponseDto> GetByIdAsync(Guid id)
    {
        var transfer = await _transferRepository.GetByIdAsync(id);
        if (transfer == null)
            throw new InvalidOperationException($"Transfer {id} not found");
        return ToResponseDto(transfer);
    }

    public async Task<TransferResponseDto> TransferAsync(TransferRequestDto request, CancellationToken cancellationToken)
    {
        var transfer = new Transfer(request.FromAccountId, request.ToAccountId, request.Amount);

        if (!await _businessDayService.IsBusinessDayAsync(DateTime.Now))
            throw new InvalidOperationException("Transfers are only allowed on business days");

        var fromAccount = await _accountRepository.GetByIdAsync(request.FromAccountId);
        if (fromAccount == null)
            throw new InvalidOperationException("Source account not found");

        var toAccount = await _accountRepository.GetByIdAsync(request.ToAccountId);
        if (toAccount == null)
            throw new InvalidOperationException("Destination account not found");

        try
        {
            fromAccount.Withdraw(request.Amount);
            toAccount.Deposit(request.Amount);
            transfer.Complete();
        }
        catch (Exception)
        {
            transfer.Reject();
            throw;
        }

        _accountRepository.Update(fromAccount);
        _accountRepository.Update(toAccount);
        _transferRepository.Add(transfer);
        await _unitOfWork.CommitAsync(cancellationToken);

        return ToResponseDto(transfer);
    }

    public async Task<List<TransferResponseDto>> GetTransferReportAsync(Guid accountId, DateTime startDate, DateTime endDate)
    {
        var report = await _transferRepository.GetByPeriodAsync(accountId, startDate, endDate);
        return report
            .Select(r => ToResponseDto(r))
            .ToList();
    }

    private static TransferResponseDto ToResponseDto(Transfer transfer)
    {
        return new TransferResponseDto(transfer.Id, transfer.FromAccountId, transfer.ToAccountId, transfer.Amount, transfer.Status, transfer.TransferredAt);
    }
}
