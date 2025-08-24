using Chu.Bank.Api.Domain.Enums;

namespace Chu.Bank.Api.Domain.DTOs;

public class TransferDtos
{
    public record TransferRequestDto(Guid FromAccountId, Guid ToAccountId, decimal Amount);
    public record TransferResponseDto(Guid Id, Guid FromAccountId, Guid ToAccountId, decimal Amount, TransferStatus Status, DateTime? TransferredAt);
}
