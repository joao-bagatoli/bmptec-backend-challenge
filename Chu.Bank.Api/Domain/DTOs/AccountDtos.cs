namespace Chu.Bank.Api.Domain.DTOs;

public class AccountDtos
{
    public record AccountRequestDto(string Holder, decimal Balance);
    public record AccountResponseDto(Guid Id, string Holder, decimal Balance);
}
