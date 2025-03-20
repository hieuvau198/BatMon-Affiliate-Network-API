using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.CampaignConversionType
{
    public class CampaignConversionTypeDto
    {
        public int CampaignConversionId { get; set; }
        public int? CampaignId { get; set; }
        public int? ConversionTypeId { get; set; }
        public decimal? CommissionAmount { get; set; }
        public string? CommissionType { get; set; }
        public int? CookieWindowDays { get; set; }

        // Navigation property data
        public string? CampaignName { get; set; }
        public string? ConversionTypeName { get; set; }
        public string? TrackingMethod { get; set; }
    }

    public class CampaignConversionTypeCreateDto
    {
        [Required]
        public int CampaignId { get; set; }

        [Required]
        public int ConversionTypeId { get; set; }

        [Required]
        public decimal CommissionAmount { get; set; }

        [Required]
        public string CommissionType { get; set; } = null!;

        public int? CookieWindowDays { get; set; }
    }

    public class CampaignConversionTypeUpdateDto
    {
        [Required]
        public int CampaignConversionId { get; set; }

        public decimal? CommissionAmount { get; set; }

        public string? CommissionType { get; set; }

        public int? CookieWindowDays { get; set; }
    }
}