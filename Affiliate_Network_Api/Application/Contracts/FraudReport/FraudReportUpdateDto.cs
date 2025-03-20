using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.FraudReport
{
    public class FraudReportUpdateDto
    {
        public int ReportId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AffectedPeriod { get; set; }
        public decimal? FinancialImpact { get; set; }
        public string? FraudPatterns { get; set; }
        public string? RecommendedActions { get; set; }
        public string? Status { get; set; }
    }
}
