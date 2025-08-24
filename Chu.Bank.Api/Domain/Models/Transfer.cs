using Chu.Bank.Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Chu.Bank.Api.Domain.Models;

public class Transfer
{
    [Key]
    public Guid Id { get; private set; }
    public Guid FromAccountId { get; private set; }
    public Guid ToAccountId { get; private set; }

    [Precision(18, 2)]
    public decimal Amount { get; set; }

    public TransferStatus Status { get; private set; }
    public DateTime? TransferredAt { get; private set; }

    public Transfer(Guid fromAccountId, Guid toAccountId, decimal amount)
    {
        if (fromAccountId == Guid.Empty ||  toAccountId == Guid.Empty)
            throw new ArgumentNullException("Invalid account IDs");

        if (fromAccountId == toAccountId)
            throw new InvalidOperationException("Source and destination accounts must be different");

        if (amount <= 0)
            throw new ArgumentOutOfRangeException("Transfer amount must be greater than zero");

        Id = Guid.NewGuid();
        FromAccountId = fromAccountId;
        ToAccountId = toAccountId;
        Amount = amount;
        Status = TransferStatus.Pending;
    }

    public void Complete()
    {
        Status = TransferStatus.Completed;
        TransferredAt = DateTime.UtcNow;
    }

    public void Reject()
    {
        Status = TransferStatus.Rejected;
    }
}
