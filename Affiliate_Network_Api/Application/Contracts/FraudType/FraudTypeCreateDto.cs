namespace Application.Contracts.FraudType
{
    public class FraudTypeCreateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? SeverityLevel { get; set; }
    }
}