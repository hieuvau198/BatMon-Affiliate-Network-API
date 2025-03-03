using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class CampaignAdvertiserUrl
{
    public int CampaignUrlId { get; set; }

    public int? CampaignId { get; set; }

    public int? AdvertiserUrlId { get; set; }

    public string? LandingPage { get; set; }

    public bool? IsActive { get; set; }

    public virtual AdvertiserUrl? AdvertiserUrl { get; set; }

    public virtual Campaign? Campaign { get; set; }

    public virtual ICollection<Promote> Promotes { get; set; } = new List<Promote>();
}
