namespace Application.Contracts.PublisherBalance
{
    public class PublisherBalanceDto
    {
        public int BalanceId { get; set; }
        public int PublisherId { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal PendingBalance { get; set; }
        public decimal LifetimeEarnings { get; set; }
        public DateOnly LastUpdated { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string PublisherName { get; set; } = string.Empty; // Extra field from Publisher entity
    }
}