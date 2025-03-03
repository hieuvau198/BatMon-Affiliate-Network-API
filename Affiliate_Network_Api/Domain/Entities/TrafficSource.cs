using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class TrafficSource
{
    public int SourceId { get; set; }

    public int? PublisherId { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Url { get; set; }

    public DateOnly? AddedDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<PromoteUrlvariant> PromoteUrlvariants { get; set; } = new List<PromoteUrlvariant>();

    public virtual Publisher? Publisher { get; set; }
}
