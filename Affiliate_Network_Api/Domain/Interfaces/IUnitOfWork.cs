using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Admin> Admins { get; }
        IGenericRepository<Advertiser> Advertisers { get; }
        IGenericRepository<AdvertiserBalance> AdvertiserBalances { get; }
        IGenericRepository<AdvertiserUrl> AdvertiserUrls { get; }
        IGenericRepository<Campaign> Campaigns { get; }
        IGenericRepository<CampaignAdvertiserUrl> CampaignAdvertiserUrls { get; }
        IGenericRepository<CampaignConversionType> CampaignConversionTypes { get; }
        IGenericRepository<CampaignPolicy> CampaignPolicies { get; }
        IGenericRepository<CampaignPublisherCommission> CampaignPublisherCommissions { get; }
        IGenericRepository<Conversion> Conversions { get; }
        IGenericRepository<ConversionType> ConversionTypes { get; }
        IGenericRepository<Currency> Currencies { get; }
        IGenericRepository<DepositRequest> DepositRequests { get; }
        IGenericRepository<FraudAdjustment> FraudAdjustments { get; }
        IGenericRepository<FraudCase> FraudCases { get; }
        IGenericRepository<FraudReport> FraudReports { get; }
        IGenericRepository<FraudType> FraudTypes { get; }
        IGenericRepository<Payment> Payments { get; }
        IGenericRepository<PaymentMethod> PaymentMethods { get; }
        IGenericRepository<PayoutRequest> PayoutRequests { get; }
        IGenericRepository<PayoutRule> PayoutRules { get; }
        IGenericRepository<Promote> Promotes { get; }
        IGenericRepository<PromoteUrlvariant> PromoteUrlvariants { get; }
        IGenericRepository<Publisher> Publishers { get; }
        IGenericRepository<PublisherBalance> PublisherBalances { get; }
        IGenericRepository<PublisherProfile> PublisherProfiles { get; }
        IGenericRepository<PublisherReferral> PublisherReferrals { get; }
        IGenericRepository<TrafficSource> TrafficSources { get; }
        IGenericRepository<WithdrawalRequest> WithdrawalRequests { get; }

        Task<int> SaveChangesAsync();
    }
}
