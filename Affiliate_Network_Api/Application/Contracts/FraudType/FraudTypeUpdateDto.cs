namespace Application.Contracts.FraudType
{
    public class FraudTypeUpdateDto
    {
        public int FraudTypeId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? SeverityLevel { get; set; }
    }
}