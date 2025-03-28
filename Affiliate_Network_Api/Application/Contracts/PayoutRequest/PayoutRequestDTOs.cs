﻿using System;

namespace Application.Contracts.PayoutRequest
{
    public class PayoutRequestDto
    {
        public int RequestId { get; set; }
        public int? PublisherId { get; set; }
        public string? PublisherName { get; set; }
        public decimal? Amount { get; set; }
        public DateOnly? RequestDate { get; set; }
        public string? Status { get; set; }
        public string? RejectionReason { get; set; }
        public int? ReviewedBy { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public string? CurrencyName { get; set; }
    }

    public class PayoutRequestCreateDto
    {
        public int? PublisherId { get; set; }
        public decimal? Amount { get; set; }
        public string CurrencyCode { get; set; } = null!;
    }

    public class PayoutRequestUpdateDto
    {
        public int RequestId { get; set; }
        public string? Status { get; set; }
        public string? RejectionReason { get; set; }
        public int? ReviewedBy { get; set; }
    }

    public class PayoutRequestFilterDto
    {
        public int? PublisherId { get; set; }
        public string? Status { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int? ReviewedBy { get; set; }
        public string? CurrencyCode { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }
}