using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Conversion
{
    public int ConversionId { get; set; }

    public int? PromoteId { get; set; }

    public int? CampaignConversionTypeId { get; set; }

    public string? ConversionType { get; set; }

    public decimal? CommissionAmount { get; set; }

    public string? ConversionValue { get; set; }

    public DateTime? ConversionTime { get; set; }

    public string? Status { get; set; }

    public string? TransactionId { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public string? DeviceType { get; set; }

    public string? Browser { get; set; }

    public string? Referrer { get; set; }

    public bool? IsUnique { get; set; }

    public bool? IsSuspicious { get; set; }

    public bool? IsFraud { get; set; }

    public DateOnly? ApprovalDate { get; set; }

    public string? RejectionReason { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public virtual CampaignConversionType? CampaignConversionType { get; set; }

    public virtual Currency CurrencyCodeNavigation { get; set; } = null!;

    public virtual ICollection<FraudCase> FraudCases { get; set; } = new List<FraudCase>();

    public virtual Promote? Promote { get; set; }
}
