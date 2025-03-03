using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class PublisherReferral
{
    public int ReferralId { get; set; }

    public int? ReferrerId { get; set; }

    public int? ReferredId { get; set; }

    public DateOnly? ReferralDate { get; set; }

    public decimal? RewardAmount { get; set; }

    public bool? IsPaid { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public virtual Currency CurrencyCodeNavigation { get; set; } = null!;

    public virtual Publisher? Referred { get; set; }

    public virtual Publisher? Referrer { get; set; }
}
