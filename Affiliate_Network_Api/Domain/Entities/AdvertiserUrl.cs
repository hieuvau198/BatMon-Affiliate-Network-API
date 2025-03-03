using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class AdvertiserUrl
{
    public int UrlId { get; set; }

    public int? AdvertiserId { get; set; }

    public string? Url { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public DateOnly? AddedDate { get; set; }

    public virtual Advertiser? Advertiser { get; set; }

    public virtual ICollection<CampaignAdvertiserUrl> CampaignAdvertiserUrls { get; set; } = new List<CampaignAdvertiserUrl>();
}
