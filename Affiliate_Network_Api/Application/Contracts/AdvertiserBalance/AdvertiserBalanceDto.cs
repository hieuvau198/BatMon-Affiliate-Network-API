using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.AdvertiserBalance
{
    public class AdvertiserBalanceDto
    {
        public int BalanceId { get; set; }
        public int? AdvertiserId { get; set; }
        public decimal? AvailableBalance { get; set; }
        public decimal? PendingBalance { get; set; }
        public decimal? LifetimeDeposits { get; set; }
        public decimal? LifetimeWithdrawals { get; set; }
        public decimal? LifetimeSpend { get; set; }
        public DateOnly? LastUpdated { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class AdvertiserBalanceCreateDto
    {
        public int? AdvertiserId { get; set; }
        public decimal? AvailableBalance { get; set; } = 0;
        public decimal? PendingBalance { get; set; } = 0;
        public string CurrencyCode { get; set; }
    }

    public class AdvertiserBalanceUpdateDto
    {
        public int BalanceId { get; set; }
        public decimal? AvailableBalance { get; set; }
        public decimal? PendingBalance { get; set; }
        public decimal? LifetimeDeposits { get; set; }
        public decimal? LifetimeWithdrawals { get; set; }
        public decimal? LifetimeSpend { get; set; }
        public string CurrencyCode { get; set; }
    }
}
