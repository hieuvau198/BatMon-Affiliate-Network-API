using System;
using System.Collections.Generic;

namespace Application.Contracts.Promote
{
    public class PromoteDto
    {
        public int PromoteId { get; set; }
        public int? PublisherId { get; set; }
        public int? CampaignId { get; set; }
        public int? CampaignAdvertiserUrlId { get; set; }
        public string? BaseTrackingUrl { get; set; }
        public DateOnly? JoinDate { get; set; }
        public bool? IsApproved { get; set; }
        public string? Status { get; set; }
        public DateOnly? LastUpdated { get; set; }
        public string? PublisherName { get; set; }
        public string? CampaignName { get; set; }
    }

    public class PromoteCreateDto
    {
        public int? PublisherId { get; set; }
        public int? CampaignId { get; set; }
        public int? CampaignAdvertiserUrlId { get; set; }
        public string? BaseTrackingUrl { get; set; }
        public bool? IsApproved { get; set; }
        public string? Status { get; set; }
    }

    public class PromoteUpdateDto
    {
        public int PromoteId { get; set; }
        public int? PublisherId { get; set; }
        public int? CampaignId { get; set; }
        public int? CampaignAdvertiserUrlId { get; set; }
        public string? BaseTrackingUrl { get; set; }
        public bool? IsApproved { get; set; }
        public string? Status { get; set; }
    }
}