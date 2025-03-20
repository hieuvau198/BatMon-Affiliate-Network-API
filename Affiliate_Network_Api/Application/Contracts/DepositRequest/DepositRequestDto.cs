namespace Application.Contracts.DepositRequest
{
    public class DepositRequestDto
    {
        public int RequestId { get; set; }
        public int? AdvertiserId { get; set; }
        public decimal? Amount { get; set; }
        public DateOnly? RequestDate { get; set; }
        public string? Status { get; set; }
        public string? PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
        public string CurrencyCode { get; set; } = null!;
        // Flattened fields for related entities
        public string? AdvertiserName { get; set; } // From Advertiser entity (assumed to be CompanyName)
        public string? CurrencyName { get; set; } // From Currency entity
    }
}