using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class FraudCase
{
    public int CaseId { get; set; }

    public int? ConversionId { get; set; }

    public int? FraudTypeId { get; set; }

    public DateOnly? DetectionDate { get; set; }

    public string? Evidence { get; set; }

    public string? Status { get; set; }

    public string? Resolution { get; set; }

    public DateOnly? ResolutionDate { get; set; }

    public int? DetectedBy { get; set; }

    public int? ResolvedBy { get; set; }

    public virtual Conversion? Conversion { get; set; }

    public virtual ICollection<FraudAdjustment> FraudAdjustments { get; set; } = new List<FraudAdjustment>();

    public virtual FraudType? FraudType { get; set; }
}
