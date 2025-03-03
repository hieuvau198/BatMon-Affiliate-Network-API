using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class PublisherBalance
{
    public int BalanceId { get; set; }

    public int? PublisherId { get; set; }

    public decimal? AvailableBalance { get; set; }

    public decimal? PendingBalance { get; set; }

    public decimal? LifetimeEarnings { get; set; }

    public DateOnly? LastUpdated { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public virtual Currency CurrencyCodeNavigation { get; set; } = null!;

    public virtual Publisher? Publisher { get; set; }
}
