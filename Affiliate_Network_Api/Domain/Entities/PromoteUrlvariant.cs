using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class PromoteUrlvariant
{
    public int VariantId { get; set; }

    public int? PromoteId { get; set; }

    public int? TrafficSourceId { get; set; }

    public string? CustomUrl { get; set; }

    public string? ShortenedUrl { get; set; }

    public string? UtmSource { get; set; }

    public string? UtmMedium { get; set; }

    public string? UtmCampaign { get; set; }

    public string? UtmContent { get; set; }

    public string? UtmTerm { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual Promote? Promote { get; set; }

    public virtual TrafficSource? TrafficSource { get; set; }
}
