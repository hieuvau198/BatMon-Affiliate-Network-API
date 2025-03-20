namespace Application.Contracts.FraudCase
{
    public class FraudCaseUpdateDto
    {
        public int CaseId { get; set; }
        public string? Status { get; set; }
        public string? Resolution { get; set; }
        public DateOnly? ResolutionDate { get; set; }
        public int? ResolvedBy { get; set; }
    }
}