using Chu.Bank.Api.Domain.Models;
using Chu.Bank.Api.Domain.Repositories;
using Chu.Bank.Api.Domain.Services;
using Chu.Bank.Api.Services;
using Moq;

namespace Chu.Bank.UnitTests.Transfers;

public class GetTransferReportTest
{
    private readonly Mock<ITransferRepository> _transferRepositoryMock;
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBusinessDayService> _businessDayServiceMock;
    private readonly ITransferService _transferService;

    public GetTransferReportTest()
    {
        _transferRepositoryMock = new Mock<ITransferRepository>();
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _businessDayServiceMock = new Mock<IBusinessDayService>();
        _transferService = new TransferService(_transferRepositoryMock.Object, _accountRepositoryMock.Object, _unitOfWorkMock.Object, _businessDayServiceMock.Object);
    }

    [Fact]
    public async Task Should_Return_Transfers_When_Report_Exists()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var transfers = new List<Transfer>
        {
            new Transfer(accountId, Guid.NewGuid(), 50m),
            new Transfer(Guid.NewGuid(), accountId, 100m)
        };

        _transferRepositoryMock.Setup(r => r.GetByPeriodAsync(accountId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(transfers);

        // Act
        var result = await _transferService.GetTransferReportAsync(accountId, DateTime.Now.AddDays(-10), DateTime.Now);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Amount == 50m);
        Assert.Contains(result, r => r.Amount == 100m);
    }

    [Fact]
    public async Task Should_Return_Empty_List_When_No_Transfers_Found()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        _transferRepositoryMock.Setup(r => r.GetByPeriodAsync(accountId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Transfer>());

        // Act
        var result = await _transferService.GetTransferReportAsync(accountId, DateTime.Now.AddDays(-10), DateTime.Now);

        // Assert
        Assert.Empty(result);
    }
}
