using System;

namespace Application.Contracts.CampaignAdvertiserUrl
{
    public class CampaignAdvertiserUrlDto
    {
        public int CampaignUrlId { get; set; }
        public int? CampaignId { get; set; }
        public int? AdvertiserUrlId { get; set; }
        public string? LandingPage { get; set; }
        public bool? IsActive { get; set; }
        public CampaignMinimalDto? Campaign { get; set; }
        public AdvertiserUrlMinimalDto? AdvertiserUrl { get; set; }
    }

    public class CampaignAdvertiserUrlCreateDto
    {
        public int? CampaignId { get; set; }
        public int? AdvertiserUrlId { get; set; }
        public string? LandingPage { get; set; }
        public bool? IsActive { get; set; } = true;
    }

    public class CampaignAdvertiserUrlUpdateDto
    {
        public int CampaignUrlId { get; set; }
        public int? CampaignId { get; set; }
        public int? AdvertiserUrlId { get; set; }
        public string? LandingPage { get; set; }
        public bool? IsActive { get; set; }
    }

    public class CampaignMinimalDto
    {
        public int CampaignId { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
    }

    public class AdvertiserUrlMinimalDto
    {
        public int UrlId { get; set; }
        public string? Url { get; set; }
        public string? Name { get; set; }
    }
}