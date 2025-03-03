using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class ConversionType
{
    public int TypeId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? TrackingMethod { get; set; }

    public bool? RequiresApproval { get; set; }

    public string? ActionType { get; set; }

    public virtual ICollection<CampaignConversionType> CampaignConversionTypes { get; set; } = new List<CampaignConversionType>();
}
