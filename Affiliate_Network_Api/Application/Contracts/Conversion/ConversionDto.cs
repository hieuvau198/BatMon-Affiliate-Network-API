using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Conversion DTOs
namespace Application.Contracts.Conversion
{
    // Main DTO for returning conversion data
    public class ConversionDto
    {
        public int ConversionId { get; set; }
        public int? PromoteId { get; set; }
        public int? CampaignConversionTypeId { get; set; }
        public string? ConversionType { get; set; }
        public decimal? CommissionAmount { get; set; }
        public string? ConversionValue { get; set; }
        public DateTime? ConversionTime { get; set; }
        public string? Status { get; set; }
        public string? TransactionId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? DeviceType { get; set; }
        public string? Browser { get; set; }
        public string? Referrer { get; set; }
        public bool? IsUnique { get; set; }
        public bool? IsSuspicious { get; set; }
        public bool? IsFraud { get; set; }
        public DateOnly? ApprovalDate { get; set; }
        public string? RejectionReason { get; set; }
        public string CurrencyCode { get; set; } = null!;
    }

    // DTO for creating new conversions
    public class ConversionCreateDto
    {
        public int? PromoteId { get; set; }
        public int? CampaignConversionTypeId { get; set; }
        public string? ConversionType { get; set; }
        public decimal? CommissionAmount { get; set; }
        public string? ConversionValue { get; set; }
        public string? TransactionId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? DeviceType { get; set; }
        public string? Browser { get; set; }
        public string? Referrer { get; set; }
        public string CurrencyCode { get; set; } = null!;
    }

    // DTO for updating existing conversions
    public class ConversionUpdateDto
    {
        public int ConversionId { get; set; }
        public string? Status { get; set; }
        public bool? IsUnique { get; set; }
        public bool? IsSuspicious { get; set; }
        public bool? IsFraud { get; set; }
        public string? RejectionReason { get; set; }
    }
}
