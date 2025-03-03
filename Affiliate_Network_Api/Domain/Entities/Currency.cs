using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Currency
{
    public string CurrencyCode { get; set; } = null!;

    public string? CurrencyName { get; set; }

    public decimal? ExchangeRate { get; set; }

    public virtual ICollection<AdvertiserBalance> AdvertiserBalances { get; set; } = new List<AdvertiserBalance>();

    public virtual ICollection<CampaignPublisherCommission> CampaignPublisherCommissions { get; set; } = new List<CampaignPublisherCommission>();

    public virtual ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();

    public virtual ICollection<Conversion> Conversions { get; set; } = new List<Conversion>();

    public virtual ICollection<DepositRequest> DepositRequests { get; set; } = new List<DepositRequest>();

    public virtual ICollection<FraudAdjustment> FraudAdjustments { get; set; } = new List<FraudAdjustment>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<PayoutRequest> PayoutRequests { get; set; } = new List<PayoutRequest>();

    public virtual ICollection<PublisherBalance> PublisherBalances { get; set; } = new List<PublisherBalance>();

    public virtual ICollection<PublisherReferral> PublisherReferrals { get; set; } = new List<PublisherReferral>();

    public virtual ICollection<WithdrawalRequest> WithdrawalRequests { get; set; } = new List<WithdrawalRequest>();
}
