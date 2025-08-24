using Chu.Bank.Api.Domain.Models;
using Chu.Bank.Api.Domain.Repositories;
using Chu.Bank.Api.Domain.Services;
using Chu.Bank.Api.Services;
using Moq;

namespace Chu.Bank.UnitTests.Transfers;

public class GetTransferByIdTest
{
    private readonly Mock<ITransferRepository> _transferRepositoryMock;
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBusinessDayService> _businessDayServiceMock;
    private readonly ITransferService _transferService;

    public GetTransferByIdTest()
    {
        _transferRepositoryMock = new Mock<ITransferRepository>();
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _businessDayServiceMock = new Mock<IBusinessDayService>();
        _transferService = new TransferService(_transferRepositoryMock.Object, _accountRepositoryMock.Object, _unitOfWorkMock.Object, _businessDayServiceMock.Object);
    }

    [Fact]
    public async Task Should_Return_Transfer_When_It_Exists()
    {
        // Arrange
        var transferId = Guid.NewGuid();
        var transfer = new Transfer(Guid.NewGuid(), Guid.NewGuid(), 100m);
        typeof(Transfer).GetProperty("Id")!.SetValue(transfer, transferId);

        _transferRepositoryMock.Setup(r => r.GetByIdAsync(transferId)).ReturnsAsync(transfer);

        // Act
        var result = await _transferService.GetByIdAsync(transferId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(transferId, result.Id);
        Assert.Equal(transfer.FromAccountId, result.FromAccountId);
        Assert.Equal(transfer.ToAccountId, result.ToAccountId);
        Assert.Equal(transfer.Amount, result.Amount);
        Assert.Equal(transfer.Status, result.Status);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Transfer_Does_Not_Exist()
    {
        // Arrange
        var transferId = Guid.NewGuid();
        _transferRepositoryMock.Setup(r => r.GetByIdAsync(transferId)).ReturnsAsync((Transfer?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _transferService.GetByIdAsync(transferId));
        Assert.Equal($"Transfer {transferId} not found", exception.Message);
    }
}
