using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public partial class AffiliateDbContext : DbContext
{
    public AffiliateDbContext()
    {
    }

    public AffiliateDbContext(DbContextOptions<AffiliateDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Advertiser> Advertisers { get; set; }

    public virtual DbSet<AdvertiserBalance> AdvertiserBalances { get; set; }

    public virtual DbSet<AdvertiserUrl> AdvertiserUrls { get; set; }

    public virtual DbSet<Campaign> Campaigns { get; set; }

    public virtual DbSet<CampaignAdvertiserUrl> CampaignAdvertiserUrls { get; set; }

    public virtual DbSet<CampaignConversionType> CampaignConversionTypes { get; set; }
    
    public virtual DbSet<CampaignPolicy> CampaignPolicies { get; set; }

    public virtual DbSet<CampaignPublisherCommission> CampaignPublisherCommissions { get; set; }

    public virtual DbSet<Conversion> Conversions { get; set; }

    public virtual DbSet<ConversionType> ConversionTypes { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<DepositRequest> DepositRequests { get; set; }

    public virtual DbSet<FraudAdjustment> FraudAdjustments { get; set; }

    public virtual DbSet<FraudCase> FraudCases { get; set; }

    public virtual DbSet<FraudReport> FraudReports { get; set; }

    public virtual DbSet<FraudType> FraudTypes { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<PayoutRequest> PayoutRequests { get; set; }

    public virtual DbSet<PayoutRule> PayoutRules { get; set; }

    public virtual DbSet<Promote> Promotes { get; set; }

    public virtual DbSet<PromoteUrlvariant> PromoteUrlvariants { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<PublisherBalance> PublisherBalances { get; set; }

    public virtual DbSet<PublisherProfile> PublisherProfiles { get; set; }

    public virtual DbSet<PublisherReferral> PublisherReferrals { get; set; }

    public virtual DbSet<TrafficSource> TrafficSources { get; set; }

    public virtual DbSet<WithdrawalRequest> WithdrawalRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__43AA41413B7F025C");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.Email, "UQ__Admin__AB6E616475B37825").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Admin__F3DBC572AD6E162B").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LastLogin)
                .HasColumnType("datetime")
                .HasColumnName("last_login");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Advertiser>(entity =>
        {
            entity.HasKey(e => e.AdvertiserId).HasName("PK__Advertis__C995B9D1A360513C");

            entity.ToTable("Advertiser");

            entity.Property(e => e.AdvertiserId).HasColumnName("advertiser_id");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(255)
                .HasColumnName("company_name");
            entity.Property(e => e.ContactName)
                .HasMaxLength(255)
                .HasColumnName("contact_name");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Industry)
                .HasMaxLength(100)
                .HasColumnName("industry");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phone_number");
            entity.Property(e => e.RegistrationDate).HasColumnName("registration_date");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.TaxId)
                .HasMaxLength(100)
                .HasColumnName("tax_id");
            entity.Property(e => e.Website)
                .HasMaxLength(255)
                .HasColumnName("website");
        });

        modelBuilder.Entity<AdvertiserBalance>(entity =>
        {
            entity.HasKey(e => e.BalanceId).HasName("PK__Advertis__18188B5B3F34B5AF");

            entity.ToTable("AdvertiserBalance");

            entity.Property(e => e.BalanceId).HasColumnName("balance_id");
            entity.Property(e => e.AdvertiserId).HasColumnName("advertiser_id");
            entity.Property(e => e.AvailableBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("available_balance");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasDefaultValue("VND")
                .HasColumnName("currency_code");
            entity.Property(e => e.LastUpdated).HasColumnName("last_updated");
            entity.Property(e => e.LifetimeDeposits)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("lifetime_deposits");
            entity.Property(e => e.LifetimeSpend)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("lifetime_spend");
            entity.Property(e => e.LifetimeWithdrawals)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("lifetime_withdrawals");
            entity.Property(e => e.PendingBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("pending_balance");

            entity.HasOne(d => d.Advertiser).WithMany(p => p.AdvertiserBalances)
                .HasForeignKey(d => d.AdvertiserId)
                .HasConstraintName("FK__Advertise__adver__52E34C9D");

            entity.HasOne(d => d.CurrencyCodeNavigation).WithMany(p => p.AdvertiserBalances)
                .HasForeignKey(d => d.CurrencyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Advertise__curre__53D770D6");
        });

        modelBuilder.Entity<AdvertiserUrl>(entity =>
        {
            entity.HasKey(e => e.UrlId).HasName("PK__Advertis__E18C06C87756A1D5");

            entity.ToTable("AdvertiserURL");

            entity.Property(e => e.UrlId).HasColumnName("url_id");
            entity.Property(e => e.AddedDate).HasColumnName("added_date");
            entity.Property(e => e.AdvertiserId).HasColumnName("advertiser_id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Url)
                .HasMaxLength(500)
                .HasColumnName("url");

            entity.HasOne(d => d.Advertiser).WithMany(p => p.AdvertiserUrls)
                .HasForeignKey(d => d.AdvertiserId)
                .HasConstraintName("FK__Advertise__adver__66EA454A");
        });

        modelBuilder.Entity<Campaign>(entity =>
        {
            entity.HasKey(e => e.CampaignId).HasName("PK__Campaign__905B681CDB761317");

            entity.ToTable("Campaign");

            entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
            entity.Property(e => e.AdvertiserId).HasColumnName("advertiser_id");
            entity.Property(e => e.Budget)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("budget");
            entity.Property(e => e.ConversionRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("conversion_rate");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasDefaultValue("VND")
                .HasColumnName("currency_code");
            entity.Property(e => e.DailyCap)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("daily_cap");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IsPrivate).HasColumnName("is_private");
            entity.Property(e => e.LastUpdated).HasColumnName("last_updated");
            entity.Property(e => e.MonthlyCap)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("monthly_cap");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TargetingCountries)
                .HasMaxLength(500)
                .HasColumnName("targeting_countries");
            entity.Property(e => e.TargetingDevices)
                .HasMaxLength(255)
                .HasColumnName("targeting_devices");

            entity.HasOne(d => d.Advertiser).WithMany(p => p.Campaigns)
                .HasForeignKey(d => d.AdvertiserId)
                .HasConstraintName("FK__Campaign__advert__6ABAD62E");

            entity.HasOne(d => d.CurrencyCodeNavigation).WithMany(p => p.Campaigns)
                .HasForeignKey(d => d.CurrencyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Campaign__curren__6BAEFA67");
        });

        modelBuilder.Entity<CampaignAdvertiserUrl>(entity =>
        {
            entity.HasKey(e => e.CampaignUrlId).HasName("PK__Campaign__6B9FFC0EBFDCF3C2");

            entity.ToTable("CampaignAdvertiserURL");

            entity.Property(e => e.CampaignUrlId).HasColumnName("campaign_url_id");
            entity.Property(e => e.AdvertiserUrlId).HasColumnName("advertiser_url_id");
            entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LandingPage)
                .HasMaxLength(500)
                .HasColumnName("landing_page");

            entity.HasOne(d => d.AdvertiserUrl).WithMany(p => p.CampaignAdvertiserUrls)
                .HasForeignKey(d => d.AdvertiserUrlId)
                .HasConstraintName("FK__CampaignA__adver__6F7F8B4B");

            entity.HasOne(d => d.Campaign).WithMany(p => p.CampaignAdvertiserUrls)
                .HasForeignKey(d => d.CampaignId)
                .HasConstraintName("FK__CampaignA__campa__6E8B6712");
        });

        modelBuilder.Entity<CampaignConversionType>(entity =>
        {
            entity.HasKey(e => e.CampaignConversionId).HasName("PK__Campaign__C4371440EA3ECFE6");

            entity.ToTable("CampaignConversionType");

            entity.Property(e => e.CampaignConversionId).HasColumnName("campaign_conversion_id");
            entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
            entity.Property(e => e.CommissionAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("commission_amount");
            entity.Property(e => e.CommissionType)
                .HasMaxLength(50)
                .HasColumnName("commission_type");
            entity.Property(e => e.ConversionTypeId).HasColumnName("conversion_type_id");
            entity.Property(e => e.CookieWindowDays).HasColumnName("cookie_window_days");

            entity.HasOne(d => d.Campaign).WithMany(p => p.CampaignConversionTypes)
                .HasForeignKey(d => d.CampaignId)
                .HasConstraintName("FK__CampaignC__campa__7FB5F314");

            entity.HasOne(d => d.ConversionType).WithMany(p => p.CampaignConversionTypes)
                .HasForeignKey(d => d.ConversionTypeId)
                .HasConstraintName("FK__CampaignC__conve__00AA174D");
        });

        modelBuilder.Entity<CampaignPolicy>(entity =>
        {
            entity.HasKey(e => e.PolicyId).HasName("PK__Campaign__47DA3F03522E6E1F");

            entity.ToTable("Campaign_Policy");

            entity.Property(e => e.PolicyId).HasColumnName("policy_id");
            entity.Property(e => e.AppliedTo)
                .HasMaxLength(255)
                .HasDefaultValue("Tat Ca Chien Dichj")
                .HasColumnName("applied_to");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasDefaultValue("Noi Dung Quy Dinh")
                .HasColumnName("description");
            entity.Property(e => e.PenaltyInfo)
                .HasMaxLength(500)
                .HasDefaultValue("Hinh Thuc Xu Phat")
                .HasColumnName("penalty_info");
            entity.Property(e => e.PolicyName)
                .HasMaxLength(255)
                .HasDefaultValue("Chinh Sach Chung")
                .HasColumnName("policy_name");
        });

        modelBuilder.Entity<CampaignPublisherCommission>(entity =>
        {
            entity.HasKey(e => e.CommissionId).HasName("PK__Campaign__D19D7CC93AF49D76");

            entity.ToTable("CampaignPublisherCommission");

            entity.Property(e => e.CommissionId).HasColumnName("commission_id");
            entity.Property(e => e.ApprovedAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("approved_amount");
            entity.Property(e => e.AvailableDate)
                .HasColumnType("datetime")
                .HasColumnName("available_date");
            entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
            entity.Property(e => e.CommissionStatus)
                .HasMaxLength(50)
                .HasColumnName("commission_status");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasDefaultValue("VND")
                .HasColumnName("currency_code");
            entity.Property(e => e.HoldoutDays)
                .HasDefaultValue(30)
                .HasColumnName("holdout_days");
            entity.Property(e => e.LastApprovalDate)
                .HasColumnType("datetime")
                .HasColumnName("last_approval_date");
            entity.Property(e => e.LastConversionDate)
                .HasColumnType("datetime")
                .HasColumnName("last_conversion_date");
            entity.Property(e => e.PaidAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("paid_amount");
            entity.Property(e => e.PendingAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("pending_amount");
            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.RejectedAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("rejected_amount");
            entity.Property(e => e.TotalAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total_amount");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Campaign).WithMany(p => p.CampaignPublisherCommissions)
                .HasForeignKey(d => d.CampaignId)
                .HasConstraintName("FK__CampaignP__campa__11D4A34F");

            entity.HasOne(d => d.CurrencyCodeNavigation).WithMany(p => p.CampaignPublisherCommissions)
                .HasForeignKey(d => d.CurrencyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CampaignP__curre__12C8C788");

            entity.HasOne(d => d.Publisher).WithMany(p => p.CampaignPublisherCommissions)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__CampaignP__publi__10E07F16");
        });

        modelBuilder.Entity<Conversion>(entity =>
        {
            entity.HasKey(e => e.ConversionId).HasName("PK__Conversi__E4E07B3F0095BF8C");

            entity.ToTable("Conversion");

            entity.Property(e => e.ConversionId).HasColumnName("conversion_id");
            entity.Property(e => e.ApprovalDate).HasColumnName("approval_date");
            entity.Property(e => e.Browser)
                .HasMaxLength(100)
                .HasColumnName("browser");
            entity.Property(e => e.CampaignConversionTypeId).HasColumnName("campaign_conversion_type_id");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.CommissionAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("commission_amount");
            entity.Property(e => e.ConversionTime)
                .HasColumnType("datetime")
                .HasColumnName("conversion_time");
            entity.Property(e => e.ConversionType)
                .HasMaxLength(100)
                .HasColumnName("conversion_type");
            entity.Property(e => e.ConversionValue)
                .HasMaxLength(255)
                .HasColumnName("conversion_value");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasDefaultValue("VND")
                .HasColumnName("currency_code");
            entity.Property(e => e.DeviceType)
                .HasMaxLength(50)
                .HasColumnName("device_type");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(45)
                .HasColumnName("ip_address");
            entity.Property(e => e.IsFraud).HasColumnName("is_fraud");
            entity.Property(e => e.IsSuspicious).HasColumnName("is_suspicious");
            entity.Property(e => e.IsUnique).HasColumnName("is_unique");
            entity.Property(e => e.PromoteId).HasColumnName("promote_id");
            entity.Property(e => e.Referrer)
                .HasMaxLength(500)
                .HasColumnName("referrer");
            entity.Property(e => e.RejectionReason)
                .HasMaxLength(255)
                .HasColumnName("rejection_reason");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(255)
                .HasColumnName("transaction_id");
            entity.Property(e => e.UserAgent)
                .HasMaxLength(500)
                .HasColumnName("user_agent");

            entity.HasOne(d => d.CampaignConversionType).WithMany(p => p.Conversions)
                .HasForeignKey(d => d.CampaignConversionTypeId)
                .HasConstraintName("FK__Conversio__campa__056ECC6A");

            entity.HasOne(d => d.CurrencyCodeNavigation).WithMany(p => p.Conversions)
                .HasForeignKey(d => d.CurrencyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Conversio__curre__0662F0A3");

            entity.HasOne(d => d.Promote).WithMany(p => p.Conversions)
                .HasForeignKey(d => d.PromoteId)
                .HasConstraintName("FK__Conversio__promo__047AA831");
        });

        modelBuilder.Entity<ConversionType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__Conversi__2C0005988F57F886");

            entity.ToTable("ConversionType");

            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.ActionType)
                .HasMaxLength(100)
                .HasColumnName("action_type");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.RequiresApproval).HasColumnName("requires_approval");
            entity.Property(e => e.TrackingMethod)
                .HasMaxLength(100)
                .HasColumnName("tracking_method");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.CurrencyCode).HasName("PK__Currency__6008D0BB7D155931");

            entity.ToTable("Currency");

            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasColumnName("currency_code");
            entity.Property(e => e.CurrencyName)
                .HasMaxLength(50)
                .HasColumnName("currency_name");
            entity.Property(e => e.ExchangeRate)
                .HasColumnType("decimal(18, 6)")
                .HasColumnName("exchange_rate");
        });

        modelBuilder.Entity<DepositRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__DepositR__18D3B90F445F2F77");

            entity.ToTable("DepositRequest");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.AdvertiserId).HasColumnName("advertiser_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasDefaultValue("VND")
                .HasColumnName("currency_code");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(100)
                .HasColumnName("payment_method");
            entity.Property(e => e.RequestDate).HasColumnName("request_date");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(255)
                .HasColumnName("transaction_id");

            entity.HasOne(d => d.Advertiser).WithMany(p => p.DepositRequests)
                .HasForeignKey(d => d.AdvertiserId)
                .HasConstraintName("FK__DepositRe__adver__57A801BA");

            entity.HasOne(d => d.CurrencyCodeNavigation).WithMany(p => p.DepositRequests)
                .HasForeignKey(d => d.CurrencyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DepositRe__curre__589C25F3");
        });

        modelBuilder.Entity<FraudAdjustment>(entity =>
        {
            entity.HasKey(e => e.AdjustmentId).HasName("PK__FraudAdj__323248498997835C");

            entity.ToTable("FraudAdjustment");

            entity.Property(e => e.AdjustmentId).HasColumnName("adjustment_id");
            entity.Property(e => e.AdjustedAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("adjusted_amount");
            entity.Property(e => e.AdjustmentDate).HasColumnName("adjustment_date");
            entity.Property(e => e.AdjustmentPercentage)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("adjustment_percentage");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasDefaultValue("VND")
                .HasColumnName("currency_code");
            entity.Property(e => e.FraudCaseId).HasColumnName("fraud_case_id");
            entity.Property(e => e.OriginalAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("original_amount");
            entity.Property(e => e.Reason)
                .HasMaxLength(500)
                .HasColumnName("reason");

            entity.HasOne(d => d.CurrencyCodeNavigation).WithMany(p => p.FraudAdjustments)
                .HasForeignKey(d => d.CurrencyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FraudAdju__curre__1D4655FB");

            entity.HasOne(d => d.FraudCase).WithMany(p => p.FraudAdjustments)
                .HasForeignKey(d => d.FraudCaseId)
                .HasConstraintName("FK__FraudAdju__fraud__1C5231C2");
        });

        modelBuilder.Entity<FraudCase>(entity =>
        {
            entity.HasKey(e => e.CaseId).HasName("PK__FraudCas__A8FF80467B5FB0B1");

            entity.ToTable("FraudCase");

            entity.Property(e => e.CaseId).HasColumnName("case_id");
            entity.Property(e => e.ConversionId).HasColumnName("conversion_id");
            entity.Property(e => e.DetectedBy).HasColumnName("detected_by");
            entity.Property(e => e.DetectionDate).HasColumnName("detection_date");
            entity.Property(e => e.Evidence).HasColumnName("evidence");
            entity.Property(e => e.FraudTypeId).HasColumnName("fraud_type_id");
            entity.Property(e => e.Resolution)
                .HasMaxLength(255)
                .HasColumnName("resolution");
            entity.Property(e => e.ResolutionDate).HasColumnName("resolution_date");
            entity.Property(e => e.ResolvedBy).HasColumnName("resolved_by");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Conversion).WithMany(p => p.FraudCases)
                .HasForeignKey(d => d.ConversionId)
                .HasConstraintName("FK__FraudCase__conve__178D7CA5");

            entity.HasOne(d => d.FraudType).WithMany(p => p.FraudCases)
                .HasForeignKey(d => d.FraudTypeId)
                .HasConstraintName("FK__FraudCase__fraud__1881A0DE");
        });

        modelBuilder.Entity<FraudReport>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__FraudRep__779B7C58115F42DC");

            entity.ToTable("FraudReport");

            entity.Property(e => e.ReportId).HasColumnName("report_id");
            entity.Property(e => e.AdvertiserId).HasColumnName("advertiser_id");
            entity.Property(e => e.AffectedPeriod)
                .HasMaxLength(100)
                .HasColumnName("affected_period");
            entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FinancialImpact)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("financial_impact");
            entity.Property(e => e.FraudPatterns).HasColumnName("fraud_patterns");
            entity.Property(e => e.IsRead).HasColumnName("is_read");
            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.ReadDate).HasColumnName("read_date");
            entity.Property(e => e.RecommendedActions).HasColumnName("recommended_actions");
            entity.Property(e => e.ReportDate).HasColumnName("report_date");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Advertiser).WithMany(p => p.FraudReports)
                .HasForeignKey(d => d.AdvertiserId)
                .HasConstraintName("FK__FraudRepo__adver__220B0B18");

            entity.HasOne(d => d.Campaign).WithMany(p => p.FraudReports)
                .HasForeignKey(d => d.CampaignId)
                .HasConstraintName("FK__FraudRepo__campa__2022C2A6");

            entity.HasOne(d => d.Publisher).WithMany(p => p.FraudReports)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__FraudRepo__publi__2116E6DF");
        });

        modelBuilder.Entity<FraudType>(entity =>
        {
            entity.HasKey(e => e.FraudTypeId).HasName("PK__FraudTyp__845EB81E3E98F2F9");

            entity.ToTable("FraudType");

            entity.Property(e => e.FraudTypeId).HasColumnName("fraud_type_id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.SeverityLevel).HasColumnName("severity_level");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__ED1FC9EAEC55E286");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasDefaultValue("VND")
                .HasColumnName("currency_code");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasColumnName("notes");
            entity.Property(e => e.PaymentDate).HasColumnName("payment_date");
            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.RequestType)
                .HasMaxLength(50)
                .HasColumnName("request_type");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(255)
                .HasColumnName("transaction_id");

            entity.HasOne(d => d.CurrencyCodeNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CurrencyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__currenc__6225902D");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK__Payment__payment__61316BF4");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.MethodId).HasName("PK__PaymentM__747727B6CE07BAF6");

            entity.ToTable("PaymentMethod");

            entity.Property(e => e.MethodId).HasColumnName("method_id");
            entity.Property(e => e.AddedDate).HasColumnName("added_date");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<PayoutRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__PayoutRe__18D3B90F0766E6C1");

            entity.ToTable("PayoutRequest");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasDefaultValue("VND")
                .HasColumnName("currency_code");
            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.RejectionReason)
                .HasMaxLength(255)
                .HasColumnName("rejection_reason");
            entity.Property(e => e.RequestDate).HasColumnName("request_date");
            entity.Property(e => e.ReviewedBy).HasColumnName("reviewed_by");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.CurrencyCodeNavigation).WithMany(p => p.PayoutRequests)
                .HasForeignKey(d => d.CurrencyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PayoutReq__curre__4D2A7347");

            entity.HasOne(d => d.Publisher).WithMany(p => p.PayoutRequests)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__PayoutReq__publi__4C364F0E");
        });

        modelBuilder.Entity<PayoutRule>(entity =>
        {
            entity.HasKey(e => e.RuleId).HasName("PK__PayoutRu__E92A929683E3A297");

            entity.ToTable("PayoutRule");

            entity.Property(e => e.RuleId).HasColumnName("rule_id");
            entity.Property(e => e.AutoPayout).HasColumnName("auto_payout");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .HasColumnName("currency");
            entity.Property(e => e.MinimumPayout)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("minimum_payout");
            entity.Property(e => e.PayoutDay).HasColumnName("payout_day");
            entity.Property(e => e.TransactionFee)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("transaction_fee");
        });

        modelBuilder.Entity<Promote>(entity =>
        {
            entity.HasKey(e => e.PromoteId).HasName("PK__Promote__8B0383CD2D6D178C");

            entity.ToTable("Promote");

            entity.Property(e => e.PromoteId).HasColumnName("promote_id");
            entity.Property(e => e.BaseTrackingUrl)
                .HasMaxLength(500)
                .HasColumnName("base_tracking_url");
            entity.Property(e => e.CampaignAdvertiserUrlId).HasColumnName("campaign_advertiser_url_id");
            entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
            entity.Property(e => e.IsApproved).HasColumnName("is_approved");
            entity.Property(e => e.JoinDate).HasColumnName("join_date");
            entity.Property(e => e.LastUpdated).HasColumnName("last_updated");
            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.CampaignAdvertiserUrl).WithMany(p => p.Promotes)
                .HasForeignKey(d => d.CampaignAdvertiserUrlId)
                .HasConstraintName("FK__Promote__campaig__7720AD13");

            entity.HasOne(d => d.Campaign).WithMany(p => p.Promotes)
                .HasForeignKey(d => d.CampaignId)
                .HasConstraintName("FK__Promote__campaig__762C88DA");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Promotes)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__Promote__publish__753864A1");
        });

        modelBuilder.Entity<PromoteUrlvariant>(entity =>
        {
            entity.HasKey(e => e.VariantId).HasName("PK__PromoteU__EACC68B7EC5BC441");

            entity.ToTable("PromoteURLVariant");

            entity.Property(e => e.VariantId).HasColumnName("variant_id");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.CustomUrl)
                .HasMaxLength(500)
                .HasColumnName("custom_url");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.PromoteId).HasColumnName("promote_id");
            entity.Property(e => e.ShortenedUrl)
                .HasMaxLength(255)
                .HasColumnName("shortened_url");
            entity.Property(e => e.TrafficSourceId).HasColumnName("traffic_source_id");
            entity.Property(e => e.UtmCampaign)
                .HasMaxLength(100)
                .HasColumnName("utm_campaign");
            entity.Property(e => e.UtmContent)
                .HasMaxLength(100)
                .HasColumnName("utm_content");
            entity.Property(e => e.UtmMedium)
                .HasMaxLength(100)
                .HasColumnName("utm_medium");
            entity.Property(e => e.UtmSource)
                .HasMaxLength(100)
                .HasColumnName("utm_source");
            entity.Property(e => e.UtmTerm)
                .HasMaxLength(100)
                .HasColumnName("utm_term");

            entity.HasOne(d => d.Promote).WithMany(p => p.PromoteUrlvariants)
                .HasForeignKey(d => d.PromoteId)
                .HasConstraintName("FK__PromoteUR__promo__79FD19BE");

            entity.HasOne(d => d.TrafficSource).WithMany(p => p.PromoteUrlvariants)
                .HasForeignKey(d => d.TrafficSourceId)
                .HasConstraintName("FK__PromoteUR__traff__7AF13DF7");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId).HasName("PK__Publishe__3263F29D13F1C2C6");

            entity.ToTable("Publisher");

            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(255)
                .HasColumnName("company_name");
            entity.Property(e => e.ContactName)
                .HasMaxLength(255)
                .HasColumnName("contact_name");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phone_number");
            entity.Property(e => e.ReferralCode)
                .HasMaxLength(100)
                .HasColumnName("referral_code");
            entity.Property(e => e.ReferredByCode)
                .HasMaxLength(100)
                .HasColumnName("referred_by_code");
            entity.Property(e => e.RegistrationDate).HasColumnName("registration_date");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.TaxId)
                .HasMaxLength(100)
                .HasColumnName("tax_id");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<PublisherBalance>(entity =>
        {
            entity.HasKey(e => e.BalanceId).HasName("PK__Publishe__18188B5BFF3717F5");

            entity.ToTable("PublisherBalance");

            entity.Property(e => e.BalanceId).HasColumnName("balance_id");
            entity.Property(e => e.AvailableBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("available_balance");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasDefaultValue("VND")
                .HasColumnName("currency_code");
            entity.Property(e => e.LastUpdated).HasColumnName("last_updated");
            entity.Property(e => e.LifetimeEarnings)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("lifetime_earnings");
            entity.Property(e => e.PendingBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("pending_balance");
            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");

            entity.HasOne(d => d.CurrencyCodeNavigation).WithMany(p => p.PublisherBalances)
                .HasForeignKey(d => d.CurrencyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Publisher__curre__467D75B8");

            entity.HasOne(d => d.Publisher).WithMany(p => p.PublisherBalances)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__Publisher__publi__4589517F");
        });

        modelBuilder.Entity<PublisherProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PK__Publishe__AEBB701FFB487DFE");

            entity.ToTable("PublisherProfile");

            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.CommunicationRating)
                .HasMaxLength(50)
                .HasColumnName("communication_rating");
            entity.Property(e => e.ExpertiseAreas)
                .HasMaxLength(500)
                .HasColumnName("expertise_areas");
            entity.Property(e => e.FraudIncidents).HasColumnName("fraud_incidents");
            entity.Property(e => e.LastUpdated).HasColumnName("last_updated");
            entity.Property(e => e.Notes)
                .HasMaxLength(1000)
                .HasColumnName("notes");
            entity.Property(e => e.PaymentReliability)
                .HasMaxLength(50)
                .HasColumnName("payment_reliability");
            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.TrafficQualityRating)
                .HasMaxLength(50)
                .HasColumnName("traffic_quality_rating");
            entity.Property(e => e.TrustScore).HasColumnName("trust_score");
            entity.Property(e => e.VerificationDate).HasColumnName("verification_date");
            entity.Property(e => e.VerificationStatus)
                .HasMaxLength(50)
                .HasColumnName("verification_status");

            entity.HasOne(d => d.Publisher).WithMany(p => p.PublisherProfiles)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__Publisher__publi__3BFFE745");
        });

        modelBuilder.Entity<PublisherReferral>(entity =>
        {
            entity.HasKey(e => e.ReferralId).HasName("PK__Publishe__62BC1805F4BF6AE5");

            entity.ToTable("PublisherReferral");

            entity.Property(e => e.ReferralId).HasColumnName("referral_id");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasDefaultValue("VND")
                .HasColumnName("currency_code");
            entity.Property(e => e.IsPaid).HasColumnName("is_paid");
            entity.Property(e => e.ReferralDate).HasColumnName("referral_date");
            entity.Property(e => e.ReferredId).HasColumnName("referred_id");
            entity.Property(e => e.ReferrerId).HasColumnName("referrer_id");
            entity.Property(e => e.RewardAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("reward_amount");

            entity.HasOne(d => d.CurrencyCodeNavigation).WithMany(p => p.PublisherReferrals)
                .HasForeignKey(d => d.CurrencyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Publisher__curre__41B8C09B");

            entity.HasOne(d => d.Referred).WithMany(p => p.PublisherReferralReferreds)
                .HasForeignKey(d => d.ReferredId)
                .HasConstraintName("FK__Publisher__refer__40C49C62");

            entity.HasOne(d => d.Referrer).WithMany(p => p.PublisherReferralReferrers)
                .HasForeignKey(d => d.ReferrerId)
                .HasConstraintName("FK__Publisher__refer__3FD07829");
        });

        modelBuilder.Entity<TrafficSource>(entity =>
        {
            entity.HasKey(e => e.SourceId).HasName("PK__TrafficS__3035A9B672B90839");

            entity.ToTable("TrafficSource");

            entity.Property(e => e.SourceId).HasColumnName("source_id");
            entity.Property(e => e.AddedDate).HasColumnName("added_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasColumnName("type");
            entity.Property(e => e.Url)
                .HasMaxLength(500)
                .HasColumnName("url");

            entity.HasOne(d => d.Publisher).WithMany(p => p.TrafficSources)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__TrafficSo__publi__725BF7F6");
        });

        modelBuilder.Entity<WithdrawalRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Withdraw__18D3B90F1C1E4BDF");

            entity.ToTable("WithdrawalRequest");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.AdvertiserId).HasColumnName("advertiser_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasDefaultValue("VND")
                .HasColumnName("currency_code");
            entity.Property(e => e.RejectionReason)
                .HasMaxLength(255)
                .HasColumnName("rejection_reason");
            entity.Property(e => e.RequestDate).HasColumnName("request_date");
            entity.Property(e => e.ReviewedBy).HasColumnName("reviewed_by");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Advertiser).WithMany(p => p.WithdrawalRequests)
                .HasForeignKey(d => d.AdvertiserId)
                .HasConstraintName("FK__Withdrawa__adver__5C6CB6D7");

            entity.HasOne(d => d.CurrencyCodeNavigation).WithMany(p => p.WithdrawalRequests)
                .HasForeignKey(d => d.CurrencyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Withdrawa__curre__5D60DB10");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
