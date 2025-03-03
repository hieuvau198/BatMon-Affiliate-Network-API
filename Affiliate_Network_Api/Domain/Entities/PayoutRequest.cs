using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class PayoutRequest
{
    public int RequestId { get; set; }

    public int? PublisherId { get; set; }

    public decimal? Amount { get; set; }

    public DateOnly? RequestDate { get; set; }

    public string? Status { get; set; }

    public string? RejectionReason { get; set; }

    public int? ReviewedBy { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public virtual Currency CurrencyCodeNavigation { get; set; } = null!;

    public virtual Publisher? Publisher { get; set; }
}
