using Chu.Bank.Api.Domain.Repositories;
using Chu.Bank.Api.Domain.Services;
using Chu.Bank.Api.Services;
using Moq;

namespace Chu.Bank.UnitTests.Accounts;

public class CreateAccountTest
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IAccountService _accountService;

    public CreateAccountTest()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _accountService = new AccountService(_accountRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Should_Create_Account_Successfully()
    {
        // Arrange
        var request = new Api.Domain.DTOs.AccountDtos.AccountRequestDto("Jane Doe", 500m);

        // Act
        var result = await _accountService.CreateAsync(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Jane Doe", result.Holder);
        Assert.Equal(500m, result.Balance);
        _accountRepositoryMock.Verify(r => r.Add(It.IsAny<Api.Domain.Models.Account>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
