namespace Application.Contracts.FraudCase
{
    public class FraudCaseDto
    {
        public int CaseId { get; set; }
        public int? ConversionId { get; set; }
        public int? FraudTypeId { get; set; }
        public DateOnly? DetectionDate { get; set; }
        public string? Evidence { get; set; }
        public string? Status { get; set; }
        public string? Resolution { get; set; }
        public DateOnly? ResolutionDate { get; set; }
        public int? DetectedBy { get; set; }
        public int? ResolvedBy { get; set; }
        // Flattened fields for related entities
        public string? ConversionTransactionId { get; set; } // From Conversion entity
        public string? FraudTypeName { get; set; } // From FraudType entity
    }
}