namespace Application.Contracts.PublisherBalance
{
    public class PublisherBalanceUpdateDto
    {
        public int BalanceId { get; set; }
        public int PublisherId { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal PendingBalance { get; set; }
        public decimal LifetimeEarnings { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
    }
}