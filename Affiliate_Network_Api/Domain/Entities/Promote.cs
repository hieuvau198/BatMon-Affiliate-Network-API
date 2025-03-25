using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Promote
{
    public int PromoteId { get; set; }

    public int? PublisherId { get; set; }

    public int? CampaignId { get; set; }

    public int? CampaignAdvertiserUrlId { get; set; }

    public string? BaseTrackingUrl { get; set; }

    public DateOnly? JoinDate { get; set; }

    public bool? IsApproved { get; set; } 

    public string? Status { get; set; }

    public DateOnly? LastUpdated { get; set; }

    public virtual Campaign? Campaign { get; set; }

    public virtual CampaignAdvertiserUrl? CampaignAdvertiserUrl { get; set; }

    public virtual ICollection<Conversion> Conversions { get; set; } = new List<Conversion>();

    public virtual ICollection<PromoteUrlvariant> PromoteUrlvariants { get; set; } = new List<PromoteUrlvariant>();

    public virtual Publisher? Publisher { get; set; }
}
