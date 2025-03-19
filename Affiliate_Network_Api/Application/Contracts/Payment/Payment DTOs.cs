using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Payment
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int? RequestId { get; set; }
        public int? PaymentMethodId { get; set; }
        public decimal? Amount { get; set; }
        public DateOnly? PaymentDate { get; set; }
        public string? TransactionId { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public string? RequestType { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public string? PaymentMethodName { get; set; }
        public string? CurrencyName { get; set; }
    }

    public class CreatePaymentDto
    {
        public int? RequestId { get; set; }
        public int? PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public DateOnly PaymentDate { get; set; }
        public string? TransactionId { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Notes { get; set; }
        public string? RequestType { get; set; }
        public string CurrencyCode { get; set; } = null!;
    }

    public class UpdatePaymentDto
    {
        public int? PaymentMethodId { get; set; }
        public decimal? Amount { get; set; }
        public DateOnly? PaymentDate { get; set; }
        public string? TransactionId { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public string CurrencyCode { get; set; } = null!;
    }

    public class PaymentFilterDto
    {
        public int? RequestId { get; set; }
        public int? PaymentMethodId { get; set; }
        public string? Status { get; set; }
        public string? RequestType { get; set; }
        public string? CurrencyCode { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }
}
