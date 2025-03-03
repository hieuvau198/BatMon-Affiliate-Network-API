using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class PaymentMethod
{
    public int MethodId { get; set; }

    public string? Type { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public DateOnly? AddedDate { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
