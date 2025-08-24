using Chu.Bank.Api.Domain.Models;
using Chu.Bank.Api.Domain.Repositories;
using Chu.Bank.Api.Domain.Services;
using Chu.Bank.Api.Services;
using Moq;

namespace Chu.Bank.UnitTests.Accounts;

public class GetAccountByIdTest
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IAccountService _accountService;

    public GetAccountByIdTest()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _accountService = new AccountService(_accountRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Should_Return_Account_When_It_Exists()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var account = new Account("John Doe", 1000m);
        typeof(Account).GetProperty("Id")!.SetValue(account, accountId);

        _accountRepositoryMock.Setup(r => r.GetByIdAsync(accountId))
            .ReturnsAsync(account);

        // Act
        var result = await _accountService.GetByIdAsync(accountId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(accountId, result.Id);
        Assert.Equal("John Doe", result.Holder);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Account_Does_Not_Exist()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        _accountRepositoryMock.Setup(r => r.GetByIdAsync(accountId))
            .ReturnsAsync((Account?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _accountService.GetByIdAsync(accountId));
        Assert.Equal($"Account {accountId} not found", exception.Message);
    }
}
