using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class Campaign
{
    public int CampaignId { get; set; }

    public int? AdvertiserId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Budget { get; set; }

    public decimal? DailyCap { get; set; }

    public decimal? MonthlyCap { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? TargetingCountries { get; set; }

    public string? TargetingDevices { get; set; }

    public string? Status { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public DateOnly? LastUpdated { get; set; }

    public bool? IsPrivate { get; set; }

    public decimal? ConversionRate { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public virtual Advertiser? Advertiser { get; set; }

    public virtual ICollection<CampaignAdvertiserUrl> CampaignAdvertiserUrls { get; set; } = new List<CampaignAdvertiserUrl>();

    public virtual ICollection<CampaignConversionType> CampaignConversionTypes { get; set; } = new List<CampaignConversionType>();

    public virtual ICollection<CampaignPublisherCommission> CampaignPublisherCommissions { get; set; } = new List<CampaignPublisherCommission>();

    public virtual Currency CurrencyCodeNavigation { get; set; } = null!;

    public virtual ICollection<FraudReport> FraudReports { get; set; } = new List<FraudReport>();

    public virtual ICollection<Promote> Promotes { get; set; } = new List<Promote>();
}
