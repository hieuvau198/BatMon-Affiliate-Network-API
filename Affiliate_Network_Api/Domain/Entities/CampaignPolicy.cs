using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class CampaignPolicy
{
    public int PolicyId { get; set; }

    public string? PolicyName { get; set; }

    public string? Description { get; set; }

    public string? PenaltyInfo { get; set; }

    public string? AppliedTo { get; set; }
}
