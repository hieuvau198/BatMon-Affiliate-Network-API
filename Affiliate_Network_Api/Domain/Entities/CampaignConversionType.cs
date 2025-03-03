using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class CampaignConversionType
{
    public int CampaignConversionId { get; set; }

    public int? CampaignId { get; set; }

    public int? ConversionTypeId { get; set; }

    public decimal? CommissionAmount { get; set; }

    public string? CommissionType { get; set; }

    public int? CookieWindowDays { get; set; }

    public virtual Campaign? Campaign { get; set; }

    public virtual ConversionType? ConversionType { get; set; }

    public virtual ICollection<Conversion> Conversions { get; set; } = new List<Conversion>();
}
