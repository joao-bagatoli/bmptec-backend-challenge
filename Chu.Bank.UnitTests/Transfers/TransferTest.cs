using Chu.Bank.Api.Domain.Enums;
using Chu.Bank.Api.Domain.Models;
using Chu.Bank.Api.Domain.Repositories;
using Chu.Bank.Api.Domain.Services;
using Chu.Bank.Api.Services;
using Moq;
using static Chu.Bank.Api.Domain.DTOs.TransferDtos;

namespace Chu.Bank.UnitTests.Transfers;

public class TransferTest
{
    private readonly Mock<ITransferRepository> _transferRepositoryMock;
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBusinessDayService> _businessDayServiceMock;
    private readonly ITransferService _transferService;

    public TransferTest()
    {
        _transferRepositoryMock = new Mock<ITransferRepository>();
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _businessDayServiceMock = new Mock<IBusinessDayService>();
        _transferService = new TransferService(_transferRepositoryMock.Object, _accountRepositoryMock.Object, _unitOfWorkMock.Object, _businessDayServiceMock.Object);
    }

    [Fact]
    public async Task Should_Transfer_When_Valid_Request()
    {
        // Arrange
        var fromAccount = new Account("John Doe", 200m);
        var toAccount = new Account("Jane Doe", 50m);

        _businessDayServiceMock.Setup(b => b.IsBusinessDayAsync(It.IsAny<DateTime>())).ReturnsAsync(true);
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(fromAccount.Id)).ReturnsAsync(fromAccount);
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(toAccount.Id)).ReturnsAsync(toAccount);

        var request = new TransferRequestDto(fromAccount.Id, toAccount.Id, 50);

        // Act
        var result = await _transferService.TransferAsync(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(150m, fromAccount.Balance);
        Assert.Equal(100m, toAccount.Balance);
        Assert.Equal(TransferStatus.Completed, result.Status);
        _accountRepositoryMock.Verify(r => r.Update(fromAccount), Times.Once);
        _accountRepositoryMock.Verify(r => r.Update(toAccount), Times.Once);
        _transferRepositoryMock.Verify(r => r.Add(It.IsAny<Transfer>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Not_Business_Day()
    {
        // Arrange
        var fromAccount = new Account("John Doe", 200m);
        var toAccount = new Account("Jane Doe", 50m);

        _businessDayServiceMock.Setup(b => b.IsBusinessDayAsync(It.IsAny<DateTime>())).ReturnsAsync(false);

        var request = new TransferRequestDto(fromAccount.Id, toAccount.Id, 50m);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _transferService.TransferAsync(request, CancellationToken.None));
        Assert.Equal("Transfers are only allowed on business days", exception.Message);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_SourceAccount_Not_Found()
    {
        // Arrange
        var toAccount = new Account("Jane Doe", 50m);

        _businessDayServiceMock.Setup(b => b.IsBusinessDayAsync(It.IsAny<DateTime>())).ReturnsAsync(true);
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Account?)null);

        var request = new TransferRequestDto(Guid.NewGuid(), toAccount.Id, 50m);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _transferService.TransferAsync(request, CancellationToken.None));
        Assert.Equal("Source account not found", exception.Message);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_DestinationAccount_Not_Found()
    {
        // Arrange
        var fromAccount = new Account("John Doe", 200m);
        var toAccountId = Guid.NewGuid();

        _businessDayServiceMock.Setup(b => b.IsBusinessDayAsync(It.IsAny<DateTime>())).ReturnsAsync(true);
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(fromAccount.Id)).ReturnsAsync(fromAccount);
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(toAccountId)).ReturnsAsync((Account?)null);

        var request = new TransferRequestDto(fromAccount.Id, toAccountId, 50m);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _transferService.TransferAsync(request, CancellationToken.None));
        Assert.Equal("Destination account not found", exception.Message);
    }

    [Fact]
    public async Task Should_Reject_Transfer_When_InsufficientBalance()
    {
        // Arrange
        var fromAccount = new Account("John Doe", 30m);
        var toAccount = new Account("Jane Doe", 50m);

        _businessDayServiceMock.Setup(b => b.IsBusinessDayAsync(It.IsAny<DateTime>())).ReturnsAsync(true);
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(fromAccount.Id)).ReturnsAsync(fromAccount);
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(toAccount.Id)).ReturnsAsync(toAccount);

        var request = new TransferRequestDto(fromAccount.Id, toAccount.Id, 50m);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _transferService.TransferAsync(request, CancellationToken.None));
        Assert.Equal("Insufficient balance", exception.Message);
        Assert.Equal(30m, fromAccount.Balance);
        Assert.Equal(50m, toAccount.Balance);
    }
}
