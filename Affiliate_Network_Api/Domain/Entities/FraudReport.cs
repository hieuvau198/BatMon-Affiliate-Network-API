using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class FraudReport
{
    public int ReportId { get; set; }

    public int? CampaignId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateOnly? ReportDate { get; set; }

    public int? PublisherId { get; set; }

    public int? AdvertiserId { get; set; }

    public string? AffectedPeriod { get; set; }

    public decimal? FinancialImpact { get; set; }

    public string? FraudPatterns { get; set; }

    public string? RecommendedActions { get; set; }

    public string? Status { get; set; }

    public bool? IsRead { get; set; }

    public DateOnly? ReadDate { get; set; }

    public virtual Advertiser? Advertiser { get; set; }

    public virtual Campaign? Campaign { get; set; }

    public virtual Publisher? Publisher { get; set; }
}
