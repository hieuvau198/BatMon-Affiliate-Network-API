using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class FraudType
{
    public int FraudTypeId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? SeverityLevel { get; set; }

    public virtual ICollection<FraudCase> FraudCases { get; set; } = new List<FraudCase>();
}
