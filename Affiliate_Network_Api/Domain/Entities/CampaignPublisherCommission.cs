using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class CampaignPublisherCommission
{
    public int CommissionId { get; set; }

    public int? PublisherId { get; set; }

    public int? CampaignId { get; set; }

    public decimal? TotalAmount { get; set; }

    public decimal? PendingAmount { get; set; }

    public decimal? ApprovedAmount { get; set; }

    public decimal? RejectedAmount { get; set; }

    public decimal? PaidAmount { get; set; }

    public DateTime? LastConversionDate { get; set; }

    public DateTime? LastApprovalDate { get; set; }

    public int? HoldoutDays { get; set; }

    public string? CommissionStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? AvailableDate { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public virtual Campaign? Campaign { get; set; }

    public virtual Currency CurrencyCodeNavigation { get; set; } = null!;

    public virtual Publisher? Publisher { get; set; }
}
