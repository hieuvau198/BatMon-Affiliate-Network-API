namespace Application.Contracts.DepositRequest
{
    public class DepositRequestCreateDto
    {
        public int? AdvertiserId { get; set; }
        public decimal? Amount { get; set; }
        public DateOnly? RequestDate { get; set; }
        public string? Status { get; set; }
        public string? PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
        public string CurrencyCode { get; set; } = null!;
    }
}