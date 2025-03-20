using System;

namespace Application.Contracts.PayoutRule
{
    public class PayoutRuleDto
    {
        public int RuleId { get; set; }
        public decimal? MinimumPayout { get; set; }
        public int? PayoutDay { get; set; }
        public string? Currency { get; set; }
        public bool? AutoPayout { get; set; }
        public decimal? TransactionFee { get; set; }
    }

    public class CreatePayoutRuleDto
    {
        public decimal? MinimumPayout { get; set; }
        public int? PayoutDay { get; set; }
        public string? Currency { get; set; }
        public bool? AutoPayout { get; set; }
        public decimal? TransactionFee { get; set; }
    }

    public class UpdatePayoutRuleDto
    {
        public decimal? MinimumPayout { get; set; }
        public int? PayoutDay { get; set; }
        public string? Currency { get; set; }
        public bool? AutoPayout { get; set; }
        public decimal? TransactionFee { get; set; }
    }

    public class PayoutRuleFilterDto
    {
        public string? Currency { get; set; }
        public bool? AutoPayout { get; set; }
    }
}