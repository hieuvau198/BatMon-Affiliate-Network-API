using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AffiliateDbContext _context;

        public IGenericRepository<Admin> Admins { get; }
        public IGenericRepository<Advertiser> Advertisers { get; }
        public IGenericRepository<AdvertiserBalance> AdvertiserBalances { get; }
        public IGenericRepository<AdvertiserUrl> AdvertiserUrls { get; }
        public IGenericRepository<Campaign> Campaigns { get; }
        public IGenericRepository<CampaignAdvertiserUrl> CampaignAdvertiserUrls { get; }
        public IGenericRepository<CampaignConversionType> CampaignConversionTypes { get; }
        public IGenericRepository<CampaignPolicy> CampaignPolicies { get; }
        public IGenericRepository<CampaignPublisherCommission> CampaignPublisherCommissions { get; }
        public IGenericRepository<Conversion> Conversions { get; }
        public IGenericRepository<ConversionType> ConversionTypes { get; }
        public IGenericRepository<Currency> Currencies { get; }
        public IGenericRepository<DepositRequest> DepositRequests { get; }
        public IGenericRepository<FraudAdjustment> FraudAdjustments { get; }
        public IGenericRepository<FraudCase> FraudCases { get; }
        public IGenericRepository<FraudReport> FraudReports { get; }
        public IGenericRepository<FraudType> FraudTypes { get; }
        public IGenericRepository<Payment> Payments { get; }
        public IGenericRepository<PaymentMethod> PaymentMethods { get; }
        public IGenericRepository<PayoutRequest> PayoutRequests { get; }
        public IGenericRepository<PayoutRule> PayoutRules { get; }
        public IGenericRepository<Promote> Promotes { get; }
        public IGenericRepository<PromoteUrlvariant> PromoteUrlvariants { get; }
        public IGenericRepository<Publisher> Publishers { get; }
        public IGenericRepository<PublisherBalance> PublisherBalances { get; }
        public IGenericRepository<PublisherProfile> PublisherProfiles { get; }
        public IGenericRepository<PublisherReferral> PublisherReferrals { get; }
        public IGenericRepository<TrafficSource> TrafficSources { get; }
        public IGenericRepository<WithdrawalRequest> WithdrawalRequests { get; }

        public UnitOfWork(AffiliateDbContext context)
        {
            _context = context;

            // Initializing repositories
            Admins = new GenericRepository<Admin>(context);
            Advertisers = new GenericRepository<Advertiser>(context);
            AdvertiserBalances = new GenericRepository<AdvertiserBalance>(context);
            AdvertiserUrls = new GenericRepository<AdvertiserUrl>(context);
            Campaigns = new GenericRepository<Campaign>(context);
            CampaignAdvertiserUrls = new GenericRepository<CampaignAdvertiserUrl>(context);
            CampaignConversionTypes = new GenericRepository<CampaignConversionType>(context);
            CampaignPolicies = new GenericRepository<CampaignPolicy>(context);
            CampaignPublisherCommissions = new GenericRepository<CampaignPublisherCommission>(context);
            Conversions = new GenericRepository<Conversion>(context);
            ConversionTypes = new GenericRepository<ConversionType>(context);
            Currencies = new GenericRepository<Currency>(context);
            DepositRequests = new GenericRepository<DepositRequest>(context);
            FraudAdjustments = new GenericRepository<FraudAdjustment>(context);
            FraudCases = new GenericRepository<FraudCase>(context);
            FraudReports = new GenericRepository<FraudReport>(context);
            FraudTypes = new GenericRepository<FraudType>(context);
            Payments = new GenericRepository<Payment>(context);
            PaymentMethods = new GenericRepository<PaymentMethod>(context);
            PayoutRequests = new GenericRepository<PayoutRequest>(context);
            PayoutRules = new GenericRepository<PayoutRule>(context);
            Promotes = new GenericRepository<Promote>(context);
            PromoteUrlvariants = new GenericRepository<PromoteUrlvariant>(context);
            Publishers = new GenericRepository<Publisher>(context);
            PublisherBalances = new GenericRepository<PublisherBalance>(context);
            PublisherProfiles = new GenericRepository<PublisherProfile>(context);
            PublisherReferrals = new GenericRepository<PublisherReferral>(context);
            TrafficSources = new GenericRepository<TrafficSource>(context);
            WithdrawalRequests = new GenericRepository<WithdrawalRequest>(context);
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
