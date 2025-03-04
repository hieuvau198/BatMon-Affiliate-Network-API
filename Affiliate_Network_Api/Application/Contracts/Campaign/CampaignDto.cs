using System;
using System.Collections.Generic;

namespace Application.Contracts.Campaign
{
    public class CampaignDto
    {
        public int CampaignId { get; set; }
        public int? AdvertiserId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Budget { get; set; }
        public decimal? DailyCap { get; set; }
        public decimal? MonthlyCap { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? TargetingCountries { get; set; }
        public string? TargetingDevices { get; set; }
        public string? Status { get; set; }
        public DateOnly? CreatedDate { get; set; }
        public DateOnly? LastUpdated { get; set; }
        public bool? IsPrivate { get; set; }
        public decimal? ConversionRate { get; set; }
        public string CurrencyCode { get; set; } = null!;

        // Navigation properties as DTOs
        public string? AdvertiserName { get; set; }
        public string? CurrencyName { get; set; }
    }
}