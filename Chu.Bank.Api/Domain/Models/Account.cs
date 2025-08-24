using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Chu.Bank.Api.Domain.Models;

public class Account
{
    [Key]
    public Guid Id { get; private set; }

    [MaxLength(100)]
    public string Holder { get; private set; }

    [Precision(18, 2)]
    public decimal Balance { get; private set; }

    public DateTime CreatedAt { get; init; }

    public Account(string holder, decimal balance)
    {
        if (string.IsNullOrWhiteSpace(holder))
            throw new ArgumentNullException("Holder is required");

        if (balance < 0)
            throw new ArgumentOutOfRangeException("Balance must be positive");

        Id = Guid.NewGuid();
        Holder = holder;
        Balance = balance;
        CreatedAt = DateTime.UtcNow;
    }

    public void Deposit(decimal amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException("Amount must be positive");
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException("Amount must be positive");
        if (Balance < amount) throw new InvalidOperationException("Insufficient balance");
        Balance -= amount;
    }
}
