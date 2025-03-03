using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? RequestId { get; set; }

    public int? PaymentMethodId { get; set; }

    public decimal? Amount { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public string? TransactionId { get; set; }

    public string? Status { get; set; }

    public string? Notes { get; set; }

    public string? RequestType { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public virtual Currency CurrencyCodeNavigation { get; set; } = null!;

    public virtual PaymentMethod? PaymentMethod { get; set; }
}
