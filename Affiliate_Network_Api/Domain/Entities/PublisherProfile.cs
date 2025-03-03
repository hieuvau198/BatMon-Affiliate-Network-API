using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class PublisherProfile
{
    public int ProfileId { get; set; }

    public int? PublisherId { get; set; }

    public int? TrustScore { get; set; }

    public string? VerificationStatus { get; set; }

    public DateOnly? VerificationDate { get; set; }

    public int? FraudIncidents { get; set; }

    public string? ExpertiseAreas { get; set; }

    public string? TrafficQualityRating { get; set; }

    public string? PaymentReliability { get; set; }

    public string? CommunicationRating { get; set; }

    public string? Notes { get; set; }

    public DateOnly? LastUpdated { get; set; }

    public virtual Publisher? Publisher { get; set; }
}
