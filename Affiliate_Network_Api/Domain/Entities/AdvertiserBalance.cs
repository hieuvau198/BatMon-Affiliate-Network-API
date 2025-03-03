using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class AdvertiserBalance
{
    public int BalanceId { get; set; }

    public int? AdvertiserId { get; set; }

    public decimal? AvailableBalance { get; set; }

    public decimal? PendingBalance { get; set; }

    public decimal? LifetimeDeposits { get; set; }

    public decimal? LifetimeWithdrawals { get; set; }

    public decimal? LifetimeSpend { get; set; }

    public DateOnly? LastUpdated { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public virtual Advertiser? Advertiser { get; set; }

    public virtual Currency CurrencyCodeNavigation { get; set; } = null!;
}
