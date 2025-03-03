using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class FraudAdjustment
{
    public int AdjustmentId { get; set; }

    public int? FraudCaseId { get; set; }

    public decimal? OriginalAmount { get; set; }

    public decimal? AdjustedAmount { get; set; }

    public decimal? AdjustmentPercentage { get; set; }

    public DateOnly? AdjustmentDate { get; set; }

    public string? Reason { get; set; }

    public int? ApprovedBy { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public virtual Currency CurrencyCodeNavigation { get; set; } = null!;

    public virtual FraudCase? FraudCase { get; set; }
}
