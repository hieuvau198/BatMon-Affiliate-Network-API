
CREATE TABLE [Admin] (
    admin_id INT PRIMARY KEY IDENTITY(1,1),
    username NVARCHAR(100) UNIQUE NOT NULL,
    email NVARCHAR(255) UNIQUE NOT NULL,
    password_hash NVARCHAR(255) NOT NULL,
    full_name NVARCHAR(255),
    [role] INT NOT NULL, -- Enum Role Stored as INT
    created_at DATETIME DEFAULT GETDATE(),
    last_login DATETIME,
    is_active BIT DEFAULT 1
);

CREATE TABLE Publisher (
    publisher_id INT PRIMARY KEY IDENTITY(1,1),
    username NVARCHAR(100),
    email NVARCHAR(255),
    password_hash NVARCHAR(255),
    company_name NVARCHAR(255),
    contact_name NVARCHAR(255),
    phone_number NVARCHAR(50),
    address NVARCHAR(500),
    tax_id NVARCHAR(100),
    registration_date DATE,
    is_active BIT,
    referred_by_code NVARCHAR(100),
    referral_code NVARCHAR(100),
    [role] INT NOT NULL -- Role as INT (Enum)
);

CREATE TABLE PublisherProfile (
    profile_id INT PRIMARY KEY IDENTITY(1,1),
    publisher_id INT,
    trust_score INT,
    verification_status NVARCHAR(50),
    verification_date DATE,
    fraud_incidents INT,
    expertise_areas NVARCHAR(500),
    traffic_quality_rating NVARCHAR(50),
    payment_reliability NVARCHAR(50),
    communication_rating NVARCHAR(50),
    notes NVARCHAR(1000),
    last_updated DATE,
    FOREIGN KEY (publisher_id) REFERENCES Publisher(publisher_id)
);

CREATE TABLE PublisherReferral (
    referral_id INT PRIMARY KEY IDENTITY(1,1),
    referrer_id INT,
    referred_id INT,
    referral_date DATE,
    reward_amount DECIMAL(18,2),
    is_paid BIT,
    FOREIGN KEY (referrer_id) REFERENCES Publisher(publisher_id),
    FOREIGN KEY (referred_id) REFERENCES Publisher(publisher_id)
);

CREATE TABLE PublisherBalance (
    balance_id INT PRIMARY KEY IDENTITY(1,1),
    publisher_id INT,
    available_balance DECIMAL(18,2),
    pending_balance DECIMAL(18,2),
    lifetime_earnings DECIMAL(18,2),
    last_updated DATE,
    FOREIGN KEY (publisher_id) REFERENCES Publisher(publisher_id)
);

CREATE TABLE PaymentMethod (
    method_id INT PRIMARY KEY IDENTITY(1,1),
    type NVARCHAR(50),
    name NVARCHAR(100),
    description NVARCHAR(255),
    is_active BIT,
    added_date DATE
);

CREATE TABLE PayoutRequest (
    request_id INT PRIMARY KEY IDENTITY(1,1),
    publisher_id INT,
    amount DECIMAL(18,2),
    request_date DATE,
    status NVARCHAR(50),
    rejection_reason NVARCHAR(255),
    reviewed_by INT,
    FOREIGN KEY (publisher_id) REFERENCES Publisher(publisher_id)
);

CREATE TABLE Advertiser (
    advertiser_id INT PRIMARY KEY IDENTITY(1,1),
    company_name NVARCHAR(255),
    contact_name NVARCHAR(255),
    email NVARCHAR(255),
    password_hash NVARCHAR(255),
    phone_number NVARCHAR(50),
    address NVARCHAR(500),
    website NVARCHAR(255),
    industry NVARCHAR(100),
    tax_id NVARCHAR(100),
    registration_date DATE,
    is_active BIT,
    [role] INT NOT NULL -- Role as INT (Enum)
);

CREATE TABLE AdvertiserBalance (
    balance_id INT PRIMARY KEY IDENTITY(1,1),
    advertiser_id INT,
    available_balance DECIMAL(18,2),
    pending_balance DECIMAL(18,2),
    lifetime_deposits DECIMAL(18,2),
    lifetime_withdrawals DECIMAL(18,2),
    lifetime_spend DECIMAL(18,2),
    last_updated DATE,
    FOREIGN KEY (advertiser_id) REFERENCES Advertiser(advertiser_id)
);

CREATE TABLE DepositRequest (
    request_id INT PRIMARY KEY IDENTITY(1,1),
    advertiser_id INT,
    amount DECIMAL(18,2),
    request_date DATE,
    status NVARCHAR(50),
    payment_method NVARCHAR(100),
    transaction_id NVARCHAR(255),
    FOREIGN KEY (advertiser_id) REFERENCES Advertiser(advertiser_id)
);

CREATE TABLE WithdrawalRequest (
    request_id INT PRIMARY KEY IDENTITY(1,1),
    advertiser_id INT,
    amount DECIMAL(18,2),
    request_date DATE,
    status NVARCHAR(50),
    rejection_reason NVARCHAR(255),
    reviewed_by INT,
    FOREIGN KEY (advertiser_id) REFERENCES Advertiser(advertiser_id)
);

CREATE TABLE Payment (
    payment_id INT PRIMARY KEY IDENTITY(1,1),
    request_id INT,
    payment_method_id INT,
    amount DECIMAL(18,2),
    payment_date DATE,
    transaction_id NVARCHAR(255),
    status NVARCHAR(50),
    notes NVARCHAR(500),
    request_type NVARCHAR(50),
    FOREIGN KEY (payment_method_id) REFERENCES PaymentMethod(method_id)
);

CREATE TABLE PayoutRule (
    rule_id INT PRIMARY KEY IDENTITY(1,1),
    minimum_payout DECIMAL(18,2),
    payout_day INT,
    currency NVARCHAR(10),
    auto_payout BIT,
    transaction_fee DECIMAL(18,2)
);

CREATE TABLE AdvertiserURL (
    url_id INT PRIMARY KEY IDENTITY(1,1),
    advertiser_id INT,
    url NVARCHAR(500),
    name NVARCHAR(255),
    description NVARCHAR(500),
    is_active BIT,
    added_date DATE,
    FOREIGN KEY (advertiser_id) REFERENCES Advertiser(advertiser_id)
);

CREATE TABLE Campaign (
    campaign_id INT PRIMARY KEY IDENTITY(1,1),
    advertiser_id INT,
    name NVARCHAR(255),
    description NVARCHAR(1000),
    budget DECIMAL(18,2),
    daily_cap DECIMAL(18,2),
    monthly_cap DECIMAL(18,2),
    start_date DATE,
    end_date DATE,
    targeting_countries NVARCHAR(500),
    targeting_devices NVARCHAR(255),
    status NVARCHAR(50),
    created_date DATE,
    last_updated DATE,
    is_private BIT,
    conversion_rate DECIMAL(10,2),
    FOREIGN KEY (advertiser_id) REFERENCES Advertiser(advertiser_id)
);

CREATE TABLE CampaignAdvertiserURL (
    campaign_url_id INT PRIMARY KEY IDENTITY(1,1),
    campaign_id INT,
    advertiser_url_id INT,
    landing_page NVARCHAR(500),
    is_active BIT,
    FOREIGN KEY (campaign_id) REFERENCES Campaign(campaign_id),
    FOREIGN KEY (advertiser_url_id) REFERENCES AdvertiserURL(url_id)
);

CREATE TABLE TrafficSource (
    source_id INT PRIMARY KEY IDENTITY(1,1),
    publisher_id INT,
    name NVARCHAR(255),
    type NVARCHAR(100),
    url NVARCHAR(500),
    added_date DATE,
    is_active BIT,
    FOREIGN KEY (publisher_id) REFERENCES Publisher(publisher_id)
);

CREATE TABLE Promote (
    promote_id INT PRIMARY KEY IDENTITY(1,1),
    publisher_id INT,
    campaign_id INT,
    campaign_advertiser_url_id INT,
    base_tracking_url NVARCHAR(500),
    join_date DATE,
    is_approved BIT,
    status NVARCHAR(50),
    last_updated DATE,
    FOREIGN KEY (publisher_id) REFERENCES Publisher(publisher_id),
    FOREIGN KEY (campaign_id) REFERENCES Campaign(campaign_id),
    FOREIGN KEY (campaign_advertiser_url_id) REFERENCES CampaignAdvertiserURL(campaign_url_id)
);

CREATE TABLE PromoteURLVariant (
    variant_id INT PRIMARY KEY IDENTITY(1,1),
    promote_id INT,
    traffic_source_id INT,
    custom_url NVARCHAR(500),
    shortened_url NVARCHAR(255),
    utm_source NVARCHAR(100),
    utm_medium NVARCHAR(100),
    utm_campaign NVARCHAR(100),
    utm_content NVARCHAR(100),
    utm_term NVARCHAR(100),
    created_date DATE,
    is_active BIT,
    FOREIGN KEY (promote_id) REFERENCES Promote(promote_id),
    FOREIGN KEY (traffic_source_id) REFERENCES TrafficSource(source_id)
);

CREATE TABLE ConversionType (
    type_id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(100),
    description NVARCHAR(500),
    tracking_method NVARCHAR(100),
    requires_approval BIT,
    action_type NVARCHAR(100)
);

CREATE TABLE CampaignConversionType (
    campaign_conversion_id INT PRIMARY KEY IDENTITY(1,1),
    campaign_id INT,
    conversion_type_id INT,
    commission_amount DECIMAL(18,2),
    commission_type NVARCHAR(50),
    cookie_window_days INT,
    FOREIGN KEY (campaign_id) REFERENCES Campaign(campaign_id),
    FOREIGN KEY (conversion_type_id) REFERENCES ConversionType(type_id)
);

CREATE TABLE Conversion (
    conversion_id INT PRIMARY KEY IDENTITY(1,1),
    promote_id INT,
    campaign_conversion_type_id INT,
    conversion_type NVARCHAR(100),
    commission_amount DECIMAL(18,2),
    conversion_value NVARCHAR(255),
    conversion_time DATETIME,
    status NVARCHAR(50),
    transaction_id NVARCHAR(255),
    ip_address NVARCHAR(45),
    user_agent NVARCHAR(500),
    country NVARCHAR(100),
    city NVARCHAR(100),
    device_type NVARCHAR(50),
    browser NVARCHAR(100),
    referrer NVARCHAR(500),
    is_unique BIT,
    is_suspicious BIT,
    is_fraud BIT,
    approval_date DATE,
    rejection_reason NVARCHAR(255),
    FOREIGN KEY (promote_id) REFERENCES Promote(promote_id),
    FOREIGN KEY (campaign_conversion_type_id) REFERENCES CampaignConversionType(campaign_conversion_id)
);

CREATE TABLE CampaignPublisherCommission (
    commission_id INT PRIMARY KEY IDENTITY(1,1),
    publisher_id INT,
    campaign_id INT,
    total_amount DECIMAL(18,2) DEFAULT 0,
    pending_amount DECIMAL(18,2) DEFAULT 0,
    approved_amount DECIMAL(18,2) DEFAULT 0,
    rejected_amount DECIMAL(18,2) DEFAULT 0,
    paid_amount DECIMAL(18,2) DEFAULT 0,
    last_conversion_date DATETIME,
    last_approval_date DATETIME,
    holdout_days INT DEFAULT 30,
    commission_status NVARCHAR(50), -- 'Active', 'On Hold', 'Suspended', 'Completed'
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
	available_date DATETIME,
    FOREIGN KEY (publisher_id) REFERENCES Publisher(publisher_id),
    FOREIGN KEY (campaign_id) REFERENCES Campaign(campaign_id)
);

CREATE TABLE FraudType (
    fraud_type_id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(100),
    description NVARCHAR(500),
    severity_level INT
);

CREATE TABLE FraudCase (
    case_id INT PRIMARY KEY IDENTITY(1,1),
    conversion_id INT,
    fraud_type_id INT,
    detection_date DATE,
    evidence NVARCHAR(MAX),
    status NVARCHAR(50),
    resolution NVARCHAR(255),
    resolution_date DATE,
    detected_by INT,
    resolved_by INT,
    FOREIGN KEY (conversion_id) REFERENCES Conversion(conversion_id),
    FOREIGN KEY (fraud_type_id) REFERENCES FraudType(fraud_type_id)
);

CREATE TABLE FraudAdjustment (
    adjustment_id INT PRIMARY KEY IDENTITY(1,1),
    fraud_case_id INT,
    original_amount DECIMAL(18,2),
    adjusted_amount DECIMAL(18,2),
    adjustment_percentage DECIMAL(10,2),
    adjustment_date DATE,
    reason NVARCHAR(500),
    approved_by INT,
    FOREIGN KEY (fraud_case_id) REFERENCES FraudCase(case_id)
);

CREATE TABLE FraudReport (
    report_id INT PRIMARY KEY IDENTITY(1,1),
    campaign_id INT,
    title NVARCHAR(255),
    description NVARCHAR(MAX),
    report_date DATE,
    publisher_id INT,
    advertiser_id INT,
    affected_period NVARCHAR(100),
    financial_impact DECIMAL(18,2),
    fraud_patterns NVARCHAR(MAX),
    recommended_actions NVARCHAR(MAX),
    status NVARCHAR(50),
    is_read BIT,
    read_date DATE,
    FOREIGN KEY (campaign_id) REFERENCES Campaign(campaign_id),
    FOREIGN KEY (publisher_id) REFERENCES Publisher(publisher_id),
    FOREIGN KEY (advertiser_id) REFERENCES Advertiser(advertiser_id)
);