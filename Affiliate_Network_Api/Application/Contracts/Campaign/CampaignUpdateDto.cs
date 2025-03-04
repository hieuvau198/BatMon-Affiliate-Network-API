using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Campaign
{
    public class CampaignUpdateDto
    {
        public int CampaignId { get; set; }
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
        public bool? IsPrivate { get; set; }
        public decimal? ConversionRate { get; set; }
        public string? CurrencyCode { get; set; }
    }
}
