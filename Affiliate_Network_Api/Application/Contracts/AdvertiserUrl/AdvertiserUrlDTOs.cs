using System;

namespace Application.Contracts.AdvertiserUrl
{
    // DTO for general use and reading
    public class AdvertiserUrlDto
    {
        public int UrlId { get; set; }
        public int? AdvertiserId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateOnly? AddedDate { get; set; }

        // Navigation properties if needed
        public string AdvertiserName { get; set; } // For showing the advertiser name without loading the full object
        public int CampaignCount { get; set; } // Count of campaigns using this URL
    }

    // DTO for creating new advertiser URLs
    public class AdvertiserUrlCreateDto
    {
        public int? AdvertiserId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
    }

    // DTO for updating existing advertiser URLs
    public class AdvertiserUrlUpdateDto
    {
        public int UrlId { get; set; }
        public int? AdvertiserId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
    }
}