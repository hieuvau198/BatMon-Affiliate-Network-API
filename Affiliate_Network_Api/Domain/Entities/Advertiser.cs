using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Advertiser
{
    public int AdvertiserId { get; set; }

    public string? CompanyName { get; set; }

    public string? ContactName { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Website { get; set; }

    public string? Industry { get; set; }

    public string? TaxId { get; set; }

    public DateOnly? RegistrationDate { get; set; }

    public bool? IsActive { get; set; }

    public int Role { get; set; }

    public virtual ICollection<AdvertiserBalance> AdvertiserBalances { get; set; } = new List<AdvertiserBalance>();

    public virtual ICollection<AdvertiserUrl> AdvertiserUrls { get; set; } = new List<AdvertiserUrl>();

    public virtual ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();

    public virtual ICollection<DepositRequest> DepositRequests { get; set; } = new List<DepositRequest>();

    public virtual ICollection<FraudReport> FraudReports { get; set; } = new List<FraudReport>();

    public virtual ICollection<WithdrawalRequest> WithdrawalRequests { get; set; } = new List<WithdrawalRequest>();
}
