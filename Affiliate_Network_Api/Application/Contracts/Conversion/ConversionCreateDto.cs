using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Conversion
{
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
}
