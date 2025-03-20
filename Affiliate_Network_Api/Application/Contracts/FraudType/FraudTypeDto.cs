namespace Application.Contracts.FraudType
{
    public class FraudTypeDto
    {
        public int FraudTypeId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? SeverityLevel { get; set; }
        // Flattened field for related entities
        public int FraudCaseCount { get; set; } // Count of related FraudCases
    }
}