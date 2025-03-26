using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.PromoteUrlvariant
{
    public class PromoteUrlvariantDto
    {
        public int VariantId { get; set; }
        public int? PromoteId { get; set; }
        public int? TrafficSourceId { get; set; }
        public string? CustomUrl { get; set; }
        public string? ShortenedUrl { get; set; }
        public string? UtmSource { get; set; }
        public string? UtmMedium { get; set; }
        public string? UtmCampaign { get; set; }
        public string? UtmContent { get; set; }
        public string? UtmTerm { get; set; }
        public DateOnly? CreatedDate { get; set; }
        public bool? IsActive { get; set; }

        // Navigation properties (for when includeRelated is true)
        public string? PromoteName { get; set; }
        public string? TrafficSourceName { get; set; }
    }

    public class PromoteUrlvariantCreateDto
    {
        public int? PromoteId { get; set; }
        public int? TrafficSourceId { get; set; }
        public string? CustomUrl { get; set; }
        public string? ShortenedUrl { get; set; }
        public string? UtmSource { get; set; }
        public string? UtmMedium { get; set; }
        public string? UtmCampaign { get; set; }
        public string? UtmContent { get; set; }
        public string? UtmTerm { get; set; }
        public bool? IsActive { get; set; }
    }

    public class PromoteUrlvariantUpdateDto
    {
        public int VariantId { get; set; }
        public int? PromoteId { get; set; }
        public int? TrafficSourceId { get; set; }
        public string? CustomUrl { get; set; }
        public string? ShortenedUrl { get; set; }
        public string? UtmSource { get; set; }
        public string? UtmMedium { get; set; }
        public string? UtmCampaign { get; set; }
        public string? UtmContent { get; set; }
        public string? UtmTerm { get; set; }
        public bool? IsActive { get; set; }
    }
}
