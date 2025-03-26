/****** Object:  Database [affiliatedb]    Script Date: 3/26/2025 2:29:13 PM ******/
CREATE DATABASE [affiliatedb]  (EDITION = 'Basic', SERVICE_OBJECTIVE = 'Basic', MAXSIZE = 2 GB) WITH CATALOG_COLLATION = SQL_Latin1_General_CP1_CI_AS, LEDGER = OFF;
GO
ALTER DATABASE [affiliatedb] SET COMPATIBILITY_LEVEL = 160
GO
ALTER DATABASE [affiliatedb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [affiliatedb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [affiliatedb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [affiliatedb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [affiliatedb] SET ARITHABORT OFF 
GO
ALTER DATABASE [affiliatedb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [affiliatedb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [affiliatedb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [affiliatedb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [affiliatedb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [affiliatedb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [affiliatedb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [affiliatedb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [affiliatedb] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [affiliatedb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [affiliatedb] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [affiliatedb] SET  MULTI_USER 
GO
ALTER DATABASE [affiliatedb] SET ENCRYPTION ON
GO
ALTER DATABASE [affiliatedb] SET QUERY_STORE = ON
GO
ALTER DATABASE [affiliatedb] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 7), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 10, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
/*** The scripts of database scoped configurations in Azure should be executed inside the target database connection. ***/
GO
-- ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 8;
GO
/****** Object:  Table [dbo].[Admin]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admin](
	[admin_id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](100) NOT NULL,
	[email] [nvarchar](255) NOT NULL,
	[password_hash] [nvarchar](255) NOT NULL,
	[full_name] [nvarchar](255) NULL,
	[role] [int] NOT NULL,
	[created_at] [datetime] NULL,
	[last_login] [datetime] NULL,
	[is_active] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[admin_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Advertiser]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Advertiser](
	[advertiser_id] [int] IDENTITY(1,1) NOT NULL,
	[company_name] [nvarchar](255) NULL,
	[contact_name] [nvarchar](255) NULL,
	[email] [nvarchar](255) NULL,
	[password_hash] [nvarchar](255) NULL,
	[phone_number] [nvarchar](50) NULL,
	[address] [nvarchar](500) NULL,
	[website] [nvarchar](255) NULL,
	[industry] [nvarchar](100) NULL,
	[tax_id] [nvarchar](100) NULL,
	[registration_date] [date] NULL,
	[is_active] [bit] NULL,
	[role] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[advertiser_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AdvertiserBalance]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdvertiserBalance](
	[balance_id] [int] IDENTITY(1,1) NOT NULL,
	[advertiser_id] [int] NULL,
	[available_balance] [decimal](18, 2) NULL,
	[pending_balance] [decimal](18, 2) NULL,
	[lifetime_deposits] [decimal](18, 2) NULL,
	[lifetime_withdrawals] [decimal](18, 2) NULL,
	[lifetime_spend] [decimal](18, 2) NULL,
	[last_updated] [date] NULL,
	[currency_code] [nvarchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[balance_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AdvertiserURL]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdvertiserURL](
	[url_id] [int] IDENTITY(1,1) NOT NULL,
	[advertiser_id] [int] NULL,
	[url] [nvarchar](500) NULL,
	[name] [nvarchar](255) NULL,
	[description] [nvarchar](500) NULL,
	[is_active] [bit] NULL,
	[added_date] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[url_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Campaign]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Campaign](
	[campaign_id] [int] IDENTITY(1,1) NOT NULL,
	[advertiser_id] [int] NULL,
	[name] [nvarchar](255) NULL,
	[description] [nvarchar](1000) NULL,
	[budget] [decimal](18, 2) NULL,
	[daily_cap] [decimal](18, 2) NULL,
	[monthly_cap] [decimal](18, 2) NULL,
	[start_date] [date] NULL,
	[end_date] [date] NULL,
	[targeting_countries] [nvarchar](500) NULL,
	[targeting_devices] [nvarchar](255) NULL,
	[status] [nvarchar](50) NULL,
	[created_date] [date] NULL,
	[last_updated] [date] NULL,
	[is_private] [bit] NULL,
	[conversion_rate] [decimal](10, 2) NULL,
	[currency_code] [nvarchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[campaign_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Campaign_Policy]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Campaign_Policy](
	[policy_id] [int] IDENTITY(1,1) NOT NULL,
	[policy_name] [nvarchar](255) NULL,
	[description] [nvarchar](1000) NULL,
	[penalty_info] [nvarchar](500) NULL,
	[applied_to] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[policy_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CampaignAdvertiserURL]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CampaignAdvertiserURL](
	[campaign_url_id] [int] IDENTITY(1,1) NOT NULL,
	[campaign_id] [int] NULL,
	[advertiser_url_id] [int] NULL,
	[landing_page] [nvarchar](500) NULL,
	[is_active] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[campaign_url_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CampaignConversionType]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CampaignConversionType](
	[campaign_conversion_id] [int] IDENTITY(1,1) NOT NULL,
	[campaign_id] [int] NULL,
	[conversion_type_id] [int] NULL,
	[commission_amount] [decimal](18, 2) NULL,
	[commission_type] [nvarchar](50) NULL,
	[cookie_window_days] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[campaign_conversion_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CampaignPublisherCommission]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CampaignPublisherCommission](
	[commission_id] [int] IDENTITY(1,1) NOT NULL,
	[publisher_id] [int] NULL,
	[campaign_id] [int] NULL,
	[total_amount] [decimal](18, 2) NULL,
	[pending_amount] [decimal](18, 2) NULL,
	[approved_amount] [decimal](18, 2) NULL,
	[rejected_amount] [decimal](18, 2) NULL,
	[paid_amount] [decimal](18, 2) NULL,
	[last_conversion_date] [datetime] NULL,
	[last_approval_date] [datetime] NULL,
	[holdout_days] [int] NULL,
	[commission_status] [nvarchar](50) NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
	[available_date] [datetime] NULL,
	[currency_code] [nvarchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[commission_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Conversion]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Conversion](
	[conversion_id] [int] IDENTITY(1,1) NOT NULL,
	[promote_id] [int] NULL,
	[campaign_conversion_type_id] [int] NULL,
	[conversion_type] [nvarchar](100) NULL,
	[commission_amount] [decimal](18, 2) NULL,
	[conversion_value] [nvarchar](255) NULL,
	[conversion_time] [datetime] NULL,
	[status] [nvarchar](50) NULL,
	[transaction_id] [nvarchar](255) NULL,
	[ip_address] [nvarchar](45) NULL,
	[user_agent] [nvarchar](500) NULL,
	[country] [nvarchar](100) NULL,
	[city] [nvarchar](100) NULL,
	[device_type] [nvarchar](50) NULL,
	[browser] [nvarchar](100) NULL,
	[referrer] [nvarchar](500) NULL,
	[is_unique] [bit] NULL,
	[is_suspicious] [bit] NULL,
	[is_fraud] [bit] NULL,
	[approval_date] [date] NULL,
	[rejection_reason] [nvarchar](255) NULL,
	[currency_code] [nvarchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[conversion_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConversionType]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConversionType](
	[type_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
	[description] [nvarchar](500) NULL,
	[tracking_method] [nvarchar](100) NULL,
	[requires_approval] [bit] NULL,
	[action_type] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[type_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Currency]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Currency](
	[currency_code] [nvarchar](10) NOT NULL,
	[currency_name] [nvarchar](50) NULL,
	[exchange_rate] [decimal](18, 6) NULL,
PRIMARY KEY CLUSTERED 
(
	[currency_code] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepositRequest]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepositRequest](
	[request_id] [int] IDENTITY(1,1) NOT NULL,
	[advertiser_id] [int] NULL,
	[amount] [decimal](18, 2) NULL,
	[request_date] [date] NULL,
	[status] [nvarchar](50) NULL,
	[payment_method] [nvarchar](100) NULL,
	[transaction_id] [nvarchar](255) NULL,
	[currency_code] [nvarchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[request_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FraudAdjustment]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FraudAdjustment](
	[adjustment_id] [int] IDENTITY(1,1) NOT NULL,
	[fraud_case_id] [int] NULL,
	[original_amount] [decimal](18, 2) NULL,
	[adjusted_amount] [decimal](18, 2) NULL,
	[adjustment_percentage] [decimal](10, 2) NULL,
	[adjustment_date] [date] NULL,
	[reason] [nvarchar](500) NULL,
	[approved_by] [int] NULL,
	[currency_code] [nvarchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[adjustment_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FraudCase]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FraudCase](
	[case_id] [int] IDENTITY(1,1) NOT NULL,
	[conversion_id] [int] NULL,
	[fraud_type_id] [int] NULL,
	[detection_date] [date] NULL,
	[evidence] [nvarchar](max) NULL,
	[status] [nvarchar](50) NULL,
	[resolution] [nvarchar](255) NULL,
	[resolution_date] [date] NULL,
	[detected_by] [int] NULL,
	[resolved_by] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[case_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FraudReport]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FraudReport](
	[report_id] [int] IDENTITY(1,1) NOT NULL,
	[campaign_id] [int] NULL,
	[title] [nvarchar](255) NULL,
	[description] [nvarchar](max) NULL,
	[report_date] [date] NULL,
	[publisher_id] [int] NULL,
	[advertiser_id] [int] NULL,
	[affected_period] [nvarchar](100) NULL,
	[financial_impact] [decimal](18, 2) NULL,
	[fraud_patterns] [nvarchar](max) NULL,
	[recommended_actions] [nvarchar](max) NULL,
	[status] [nvarchar](50) NULL,
	[is_read] [bit] NULL,
	[read_date] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[report_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FraudType]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FraudType](
	[fraud_type_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
	[description] [nvarchar](500) NULL,
	[severity_level] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[fraud_type_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[payment_id] [int] IDENTITY(1,1) NOT NULL,
	[request_id] [int] NULL,
	[payment_method_id] [int] NULL,
	[amount] [decimal](18, 2) NULL,
	[payment_date] [date] NULL,
	[transaction_id] [nvarchar](255) NULL,
	[status] [nvarchar](50) NULL,
	[notes] [nvarchar](500) NULL,
	[request_type] [nvarchar](50) NULL,
	[currency_code] [nvarchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[payment_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethod]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethod](
	[method_id] [int] IDENTITY(1,1) NOT NULL,
	[type] [nvarchar](50) NULL,
	[name] [nvarchar](100) NULL,
	[description] [nvarchar](255) NULL,
	[is_active] [bit] NULL,
	[added_date] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[method_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PayoutRequest]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PayoutRequest](
	[request_id] [int] IDENTITY(1,1) NOT NULL,
	[publisher_id] [int] NULL,
	[amount] [decimal](18, 2) NULL,
	[request_date] [date] NULL,
	[status] [nvarchar](50) NULL,
	[rejection_reason] [nvarchar](255) NULL,
	[reviewed_by] [int] NULL,
	[currency_code] [nvarchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[request_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[PayoutRule]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PayoutRule](
	[rule_id] [int] IDENTITY(1,1) NOT NULL,
	[minimum_payout] [decimal](18, 2) NULL,
	[payout_day] [int] NULL,
	[currency] [nvarchar](10) NULL,
	[auto_payout] [bit] NULL,
	[transaction_fee] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[rule_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Promote]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Promote](
	[promote_id] [int] IDENTITY(1,1) NOT NULL,
	[publisher_id] [int] NULL,
	[campaign_id] [int] NULL,
	[campaign_advertiser_url_id] [int] NULL,
	[base_tracking_url] [nvarchar](500) NULL,
	[join_date] [date] NULL,
	[is_approved] [bit] NULL,
	[status] [nvarchar](50) NULL,
	[last_updated] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[promote_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PromoteURLVariant]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PromoteURLVariant](
	[variant_id] [int] IDENTITY(1,1) NOT NULL,
	[promote_id] [int] NULL,
	[traffic_source_id] [int] NULL,
	[custom_url] [nvarchar](500) NULL,
	[shortened_url] [nvarchar](255) NULL,
	[utm_source] [nvarchar](100) NULL,
	[utm_medium] [nvarchar](100) NULL,
	[utm_campaign] [nvarchar](100) NULL,
	[utm_content] [nvarchar](100) NULL,
	[utm_term] [nvarchar](100) NULL,
	[created_date] [date] NULL,
	[is_active] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[variant_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Publisher]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Publisher](
	[publisher_id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](100) NULL,
	[email] [nvarchar](255) NULL,
	[password_hash] [nvarchar](255) NULL,
	[company_name] [nvarchar](255) NULL,
	[contact_name] [nvarchar](255) NULL,
	[phone_number] [nvarchar](50) NULL,
	[address] [nvarchar](500) NULL,
	[tax_id] [nvarchar](100) NULL,
	[registration_date] [date] NULL,
	[is_active] [bit] NULL,
	[referred_by_code] [nvarchar](100) NULL,
	[referral_code] [nvarchar](100) NULL,
	[role] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[publisher_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PublisherBalance]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PublisherBalance](
	[balance_id] [int] IDENTITY(1,1) NOT NULL,
	[publisher_id] [int] NULL,
	[available_balance] [decimal](18, 2) NULL,
	[pending_balance] [decimal](18, 2) NULL,
	[lifetime_earnings] [decimal](18, 2) NULL,
	[last_updated] [date] NULL,
	[currency_code] [nvarchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[balance_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PublisherProfile]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PublisherProfile](
	[profile_id] [int] IDENTITY(1,1) NOT NULL,
	[publisher_id] [int] NULL,
	[trust_score] [int] NULL,
	[verification_status] [nvarchar](50) NULL,
	[verification_date] [date] NULL,
	[fraud_incidents] [int] NULL,
	[expertise_areas] [nvarchar](500) NULL,
	[traffic_quality_rating] [nvarchar](50) NULL,
	[payment_reliability] [nvarchar](50) NULL,
	[communication_rating] [nvarchar](50) NULL,
	[notes] [nvarchar](1000) NULL,
	[last_updated] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[profile_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PublisherReferral]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PublisherReferral](
	[referral_id] [int] IDENTITY(1,1) NOT NULL,
	[referrer_id] [int] NULL,
	[referred_id] [int] NULL,
	[referral_date] [date] NULL,
	[reward_amount] [decimal](18, 2) NULL,
	[is_paid] [bit] NULL,
	[currency_code] [nvarchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[referral_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrafficSource]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrafficSource](
	[source_id] [int] IDENTITY(1,1) NOT NULL,
	[publisher_id] [int] NULL,
	[name] [nvarchar](255) NULL,
	[type] [nvarchar](100) NULL,
	[url] [nvarchar](500) NULL,
	[added_date] [date] NULL,
	[is_active] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[source_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WithdrawalRequest]    Script Date: 3/26/2025 2:29:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WithdrawalRequest](
	[request_id] [int] IDENTITY(1,1) NOT NULL,
	[advertiser_id] [int] NULL,
	[amount] [decimal](18, 2) NULL,
	[request_date] [date] NULL,
	[status] [nvarchar](50) NULL,
	[rejection_reason] [nvarchar](255) NULL,
	[reviewed_by] [int] NULL,
	[currency_code] [nvarchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[request_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Admin] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Admin] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[AdvertiserBalance] ADD  DEFAULT ('VND') FOR [currency_code]
GO
ALTER TABLE [dbo].[Campaign] ADD  DEFAULT ('VND') FOR [currency_code]
GO
ALTER TABLE [dbo].[Campaign_Policy] ADD  DEFAULT ('Chinh Sach Chung') FOR [policy_name]
GO
ALTER TABLE [dbo].[Campaign_Policy] ADD  DEFAULT ('Noi Dung Quy Dinh') FOR [description]
GO
ALTER TABLE [dbo].[Campaign_Policy] ADD  DEFAULT ('Hinh Thuc Xu Phat') FOR [penalty_info]
GO
ALTER TABLE [dbo].[Campaign_Policy] ADD  DEFAULT ('Tat Ca Chien Dichj') FOR [applied_to]
GO
ALTER TABLE [dbo].[CampaignPublisherCommission] ADD  DEFAULT ((0)) FOR [total_amount]
GO
ALTER TABLE [dbo].[CampaignPublisherCommission] ADD  DEFAULT ((0)) FOR [pending_amount]
GO
ALTER TABLE [dbo].[CampaignPublisherCommission] ADD  DEFAULT ((0)) FOR [approved_amount]
GO
ALTER TABLE [dbo].[CampaignPublisherCommission] ADD  DEFAULT ((0)) FOR [rejected_amount]
GO
ALTER TABLE [dbo].[CampaignPublisherCommission] ADD  DEFAULT ((0)) FOR [paid_amount]
GO
ALTER TABLE [dbo].[CampaignPublisherCommission] ADD  DEFAULT ((30)) FOR [holdout_days]
GO
ALTER TABLE [dbo].[CampaignPublisherCommission] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[CampaignPublisherCommission] ADD  DEFAULT ('VND') FOR [currency_code]
GO
ALTER TABLE [dbo].[Conversion] ADD  DEFAULT ('VND') FOR [currency_code]
GO
ALTER TABLE [dbo].[DepositRequest] ADD  DEFAULT ('VND') FOR [currency_code]
GO
ALTER TABLE [dbo].[FraudAdjustment] ADD  DEFAULT ('VND') FOR [currency_code]
GO
ALTER TABLE [dbo].[Payment] ADD  DEFAULT ('VND') FOR [currency_code]
GO
ALTER TABLE [dbo].[PayoutRequest] ADD  DEFAULT ('VND') FOR [currency_code]
GO
ALTER TABLE [dbo].[PublisherBalance] ADD  DEFAULT ('VND') FOR [currency_code]
GO
ALTER TABLE [dbo].[PublisherReferral] ADD  DEFAULT ('VND') FOR [currency_code]
GO
ALTER TABLE [dbo].[WithdrawalRequest] ADD  DEFAULT ('VND') FOR [currency_code]
GO
ALTER TABLE [dbo].[AdvertiserBalance]  WITH CHECK ADD FOREIGN KEY([advertiser_id])
REFERENCES [dbo].[Advertiser] ([advertiser_id])
GO
ALTER TABLE [dbo].[AdvertiserBalance]  WITH CHECK ADD FOREIGN KEY([currency_code])
REFERENCES [dbo].[Currency] ([currency_code])
GO
ALTER TABLE [dbo].[AdvertiserURL]  WITH CHECK ADD FOREIGN KEY([advertiser_id])
REFERENCES [dbo].[Advertiser] ([advertiser_id])
GO
ALTER TABLE [dbo].[Campaign]  WITH CHECK ADD FOREIGN KEY([advertiser_id])
REFERENCES [dbo].[Advertiser] ([advertiser_id])
GO
ALTER TABLE [dbo].[Campaign]  WITH CHECK ADD FOREIGN KEY([currency_code])
REFERENCES [dbo].[Currency] ([currency_code])
GO
ALTER TABLE [dbo].[CampaignAdvertiserURL]  WITH CHECK ADD FOREIGN KEY([advertiser_url_id])
REFERENCES [dbo].[AdvertiserURL] ([url_id])
GO
ALTER TABLE [dbo].[CampaignAdvertiserURL]  WITH CHECK ADD FOREIGN KEY([campaign_id])
REFERENCES [dbo].[Campaign] ([campaign_id])
GO
ALTER TABLE [dbo].[CampaignConversionType]  WITH CHECK ADD FOREIGN KEY([campaign_id])
REFERENCES [dbo].[Campaign] ([campaign_id])
GO
ALTER TABLE [dbo].[CampaignConversionType]  WITH CHECK ADD FOREIGN KEY([conversion_type_id])
REFERENCES [dbo].[ConversionType] ([type_id])
GO
ALTER TABLE [dbo].[CampaignPublisherCommission]  WITH CHECK ADD FOREIGN KEY([campaign_id])
REFERENCES [dbo].[Campaign] ([campaign_id])
GO
ALTER TABLE [dbo].[CampaignPublisherCommission]  WITH CHECK ADD FOREIGN KEY([currency_code])
REFERENCES [dbo].[Currency] ([currency_code])
GO
ALTER TABLE [dbo].[CampaignPublisherCommission]  WITH CHECK ADD FOREIGN KEY([publisher_id])
REFERENCES [dbo].[Publisher] ([publisher_id])
GO
ALTER TABLE [dbo].[Conversion]  WITH CHECK ADD FOREIGN KEY([campaign_conversion_type_id])
REFERENCES [dbo].[CampaignConversionType] ([campaign_conversion_id])
GO
ALTER TABLE [dbo].[Conversion]  WITH CHECK ADD FOREIGN KEY([currency_code])
REFERENCES [dbo].[Currency] ([currency_code])
GO
ALTER TABLE [dbo].[Conversion]  WITH CHECK ADD FOREIGN KEY([promote_id])
REFERENCES [dbo].[Promote] ([promote_id])
GO
ALTER TABLE [dbo].[DepositRequest]  WITH CHECK ADD FOREIGN KEY([advertiser_id])
REFERENCES [dbo].[Advertiser] ([advertiser_id])
GO
ALTER TABLE [dbo].[DepositRequest]  WITH CHECK ADD FOREIGN KEY([currency_code])
REFERENCES [dbo].[Currency] ([currency_code])
GO
ALTER TABLE [dbo].[FraudAdjustment]  WITH CHECK ADD FOREIGN KEY([currency_code])
REFERENCES [dbo].[Currency] ([currency_code])
GO
ALTER TABLE [dbo].[FraudAdjustment]  WITH CHECK ADD FOREIGN KEY([fraud_case_id])
REFERENCES [dbo].[FraudCase] ([case_id])
GO
ALTER TABLE [dbo].[FraudCase]  WITH CHECK ADD FOREIGN KEY([conversion_id])
REFERENCES [dbo].[Conversion] ([conversion_id])
GO
ALTER TABLE [dbo].[FraudCase]  WITH CHECK ADD FOREIGN KEY([fraud_type_id])
REFERENCES [dbo].[FraudType] ([fraud_type_id])
GO
ALTER TABLE [dbo].[FraudReport]  WITH CHECK ADD FOREIGN KEY([advertiser_id])
REFERENCES [dbo].[Advertiser] ([advertiser_id])
GO
ALTER TABLE [dbo].[FraudReport]  WITH CHECK ADD FOREIGN KEY([campaign_id])
REFERENCES [dbo].[Campaign] ([campaign_id])
GO
ALTER TABLE [dbo].[FraudReport]  WITH CHECK ADD FOREIGN KEY([publisher_id])
REFERENCES [dbo].[Publisher] ([publisher_id])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([currency_code])
REFERENCES [dbo].[Currency] ([currency_code])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([payment_method_id])
REFERENCES [dbo].[PaymentMethod] ([method_id])
GO
ALTER TABLE [dbo].[PayoutRequest]  WITH CHECK ADD FOREIGN KEY([currency_code])
REFERENCES [dbo].[Currency] ([currency_code])
GO
ALTER TABLE [dbo].[PayoutRequest]  WITH CHECK ADD FOREIGN KEY([publisher_id])
REFERENCES [dbo].[Publisher] ([publisher_id])
GO
ALTER TABLE [dbo].[Promote]  WITH CHECK ADD FOREIGN KEY([campaign_id])
REFERENCES [dbo].[Campaign] ([campaign_id])
GO
ALTER TABLE [dbo].[Promote]  WITH CHECK ADD FOREIGN KEY([campaign_advertiser_url_id])
REFERENCES [dbo].[CampaignAdvertiserURL] ([campaign_url_id])
GO
ALTER TABLE [dbo].[Promote]  WITH CHECK ADD FOREIGN KEY([publisher_id])
REFERENCES [dbo].[Publisher] ([publisher_id])
GO
ALTER TABLE [dbo].[PromoteURLVariant]  WITH CHECK ADD FOREIGN KEY([promote_id])
REFERENCES [dbo].[Promote] ([promote_id])
GO
ALTER TABLE [dbo].[PromoteURLVariant]  WITH CHECK ADD FOREIGN KEY([traffic_source_id])
REFERENCES [dbo].[TrafficSource] ([source_id])
GO
ALTER TABLE [dbo].[PublisherBalance]  WITH CHECK ADD FOREIGN KEY([currency_code])
REFERENCES [dbo].[Currency] ([currency_code])
GO
ALTER TABLE [dbo].[PublisherBalance]  WITH CHECK ADD FOREIGN KEY([publisher_id])
REFERENCES [dbo].[Publisher] ([publisher_id])
GO
ALTER TABLE [dbo].[PublisherProfile]  WITH CHECK ADD FOREIGN KEY([publisher_id])
REFERENCES [dbo].[Publisher] ([publisher_id])
GO
ALTER TABLE [dbo].[PublisherReferral]  WITH CHECK ADD FOREIGN KEY([currency_code])
REFERENCES [dbo].[Currency] ([currency_code])
GO
ALTER TABLE [dbo].[PublisherReferral]  WITH CHECK ADD FOREIGN KEY([referrer_id])
REFERENCES [dbo].[Publisher] ([publisher_id])
GO
ALTER TABLE [dbo].[PublisherReferral]  WITH CHECK ADD FOREIGN KEY([referred_id])
REFERENCES [dbo].[Publisher] ([publisher_id])
GO
ALTER TABLE [dbo].[TrafficSource]  WITH CHECK ADD FOREIGN KEY([publisher_id])
REFERENCES [dbo].[Publisher] ([publisher_id])
GO
ALTER TABLE [dbo].[WithdrawalRequest]  WITH CHECK ADD FOREIGN KEY([advertiser_id])
REFERENCES [dbo].[Advertiser] ([advertiser_id])
GO
ALTER TABLE [dbo].[WithdrawalRequest]  WITH CHECK ADD FOREIGN KEY([currency_code])
REFERENCES [dbo].[Currency] ([currency_code])
GO
ALTER DATABASE [affiliatedb] SET  READ_WRITE 
GO
