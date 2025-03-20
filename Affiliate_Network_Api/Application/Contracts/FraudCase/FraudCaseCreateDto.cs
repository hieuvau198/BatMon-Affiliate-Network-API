namespace Application.Contracts.FraudCase
{
    public class FraudCaseCreateDto
    {
        public int? ConversionId { get; set; }
        public int? FraudTypeId { get; set; }
        public DateOnly? DetectionDate { get; set; }
        public string? Evidence { get; set; }
        public string? Status { get; set; }
        public int? DetectedBy { get; set; }
    }
}