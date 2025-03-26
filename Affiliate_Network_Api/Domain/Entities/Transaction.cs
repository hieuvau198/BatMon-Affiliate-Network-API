using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public string SenderName { get; set; } = null!;

    public string ReceiverName { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public DateOnly PaymentDate { get; set; }

    public string? Notes { get; set; }

    public string? TransactionRef { get; set; }

    public int? SenderId { get; set; }

    public int? ReceiverId { get; set; }
}
