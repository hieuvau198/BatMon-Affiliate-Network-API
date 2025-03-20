using System;

namespace Application.Contracts.WithdrawalRequest
{    public abstract class WithdrawalRequestBaseDto
    {
        public decimal? Amount { get; set; }
        public string CurrencyCode { get; set; } = null!;
    }

    public class WithdrawalRequestDto : WithdrawalRequestBaseDto
    {
        public int RequestId { get; set; }
        public int? AdvertiserId { get; set; }
        public string? AdvertiserName { get; set; }
        public DateOnly? RequestDate { get; set; }
        public string? Status { get; set; }
        public string? RejectionReason { get; set; }
        public int? ReviewedBy { get; set; }
        public string? CurrencyName { get; set; }
    }

    public class CreateWithdrawalRequestDto : WithdrawalRequestBaseDto
    {
        public int AdvertiserId { get; set; }
    }

    public class UpdateWithdrawalRequestDto
    {
        public string? Status { get; set; }
        public string? RejectionReason { get; set; }
        public int? ReviewedBy { get; set; }
    }

    public class WithdrawalRequestFilterDto
    {
        public int? AdvertiserId { get; set; }
        public string? Status { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public string? CurrencyCode { get; set; }
    }
}