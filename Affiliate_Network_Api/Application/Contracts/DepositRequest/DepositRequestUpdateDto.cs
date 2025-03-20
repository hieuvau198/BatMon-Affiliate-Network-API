namespace Application.Contracts.DepositRequest
{
    public class DepositRequestUpdateDto
    {
        public int RequestId { get; set; }
        public string? Status { get; set; }
        public string? TransactionId { get; set; }
    }
}