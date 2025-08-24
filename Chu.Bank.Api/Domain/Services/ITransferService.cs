using static Chu.Bank.Api.Domain.DTOs.TransferDtos;

namespace Chu.Bank.Api.Domain.Services;

public interface ITransferService
{
    Task<TransferResponseDto> GetByIdAsync(Guid id);
    Task<TransferResponseDto> TransferAsync(TransferRequestDto request, CancellationToken cancellationToken);
    Task<List<TransferResponseDto>> GetTransferReportAsync(Guid accountId, DateTime startDate, DateTime endDate);
}
