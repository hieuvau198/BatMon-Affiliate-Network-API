using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class PayoutRule
{
    public int RuleId { get; set; }

    public decimal? MinimumPayout { get; set; }

    public int? PayoutDay { get; set; }

    public string? Currency { get; set; }

    public bool? AutoPayout { get; set; }

    public decimal? TransactionFee { get; set; }
}
