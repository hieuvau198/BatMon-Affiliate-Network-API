using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class Publisher
{
    public int PublisherId { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? CompanyName { get; set; }

    public string? ContactName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? TaxId { get; set; }

    public DateOnly? RegistrationDate { get; set; }

    public bool? IsActive { get; set; }

    public string? ReferredByCode { get; set; }

    public string? ReferralCode { get; set; }

    public int Role { get; set; }

    public virtual ICollection<CampaignPublisherCommission> CampaignPublisherCommissions { get; set; } = new List<CampaignPublisherCommission>();

    public virtual ICollection<FraudReport> FraudReports { get; set; } = new List<FraudReport>();

    public virtual ICollection<PayoutRequest> PayoutRequests { get; set; } = new List<PayoutRequest>();

    public virtual ICollection<Promote> Promotes { get; set; } = new List<Promote>();

    public virtual ICollection<PublisherBalance> PublisherBalances { get; set; } = new List<PublisherBalance>();

    public virtual ICollection<PublisherProfile> PublisherProfiles { get; set; } = new List<PublisherProfile>();

    public virtual ICollection<PublisherReferral> PublisherReferralReferreds { get; set; } = new List<PublisherReferral>();

    public virtual ICollection<PublisherReferral> PublisherReferralReferrers { get; set; } = new List<PublisherReferral>();

    public virtual ICollection<TrafficSource> TrafficSources { get; set; } = new List<TrafficSource>();
}
