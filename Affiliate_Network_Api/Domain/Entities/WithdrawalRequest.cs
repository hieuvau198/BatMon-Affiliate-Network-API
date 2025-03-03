using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class WithdrawalRequest
{
    public int RequestId { get; set; }

    public int? AdvertiserId { get; set; }

    public decimal? Amount { get; set; }

    public DateOnly? RequestDate { get; set; }

    public string? Status { get; set; }

    public string? RejectionReason { get; set; }

    public int? ReviewedBy { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public virtual Advertiser? Advertiser { get; set; }

    public virtual Currency CurrencyCodeNavigation { get; set; } = null!;
}
