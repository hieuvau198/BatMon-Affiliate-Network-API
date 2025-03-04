
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

CREATE TABLE Currency (
    currency_code NVARCHAR(10) PRIMARY KEY, -- 'USD', 'EUR', 'GBP'
    currency_name NVARCHAR(50),
    exchange_rate DECIMAL(18,6) -- Example: 1 USD = 0.85 EUR
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
	currency_code NVARCHAR(10) NOT NULL DEFAULT 'VND',
    FOREIGN KEY (referrer_id) REFERENCES Publisher(publisher_id),
    FOREIGN KEY (referred_id) REFERENCES Publisher(publisher_id),
	FOREIGN KEY (currency_code) REFERENCES Currency(currency_code)
);

CREATE TABLE PublisherBalance (
    balance_id INT PRIMARY KEY IDENTITY(1,1),
    publisher_id INT,
    available_balance DECIMAL(18,2),
    pending_balance DECIMAL(18,2),
    lifetime_earnings DECIMAL(18,2),
    last_updated DATE,
	currency_code NVARCHAR(10) NOT NULL DEFAULT 'VND',
    FOREIGN KEY (publisher_id) REFERENCES Publisher(publisher_id),
	FOREIGN KEY (currency_code) REFERENCES Currency(currency_code)
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
	currency_code NVARCHAR(10) NOT NULL DEFAULT 'VND',
    FOREIGN KEY (publisher_id) REFERENCES Publisher(publisher_id),
	FOREIGN KEY (currency_code) REFERENCES Currency(currency_code)
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
	currency_code NVARCHAR(10) NOT NULL DEFAULT 'VND',
    FOREIGN KEY (advertiser_id) REFERENCES Advertiser(advertiser_id),
	FOREIGN KEY (currency_code) REFERENCES Currency(currency_code)
);

CREATE TABLE DepositRequest (
    request_id INT PRIMARY KEY IDENTITY(1,1),
    advertiser_id INT,
    amount DECIMAL(18,2),
    request_date DATE,
    status NVARCHAR(50),
    payment_method NVARCHAR(100),
    transaction_id NVARCHAR(255),
	currency_code NVARCHAR(10) NOT NULL DEFAULT 'VND',
    FOREIGN KEY (advertiser_id) REFERENCES Advertiser(advertiser_id),
	FOREIGN KEY (currency_code) REFERENCES Currency(currency_code)
);

CREATE TABLE WithdrawalRequest (
    request_id INT PRIMARY KEY IDENTITY(1,1),
    advertiser_id INT,
    amount DECIMAL(18,2),
    request_date DATE,
    status NVARCHAR(50),
    rejection_reason NVARCHAR(255),
    reviewed_by INT,
	currency_code NVARCHAR(10) NOT NULL DEFAULT 'VND',
    FOREIGN KEY (advertiser_id) REFERENCES Advertiser(advertiser_id),
	FOREIGN KEY (currency_code) REFERENCES Currency(currency_code)
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
	currency_code NVARCHAR(10) NOT NULL DEFAULT 'VND',
    FOREIGN KEY (payment_method_id) REFERENCES PaymentMethod(method_id),
	FOREIGN KEY (currency_code) REFERENCES Currency(currency_code)
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
	currency_code NVARCHAR(10) NOT NULL DEFAULT 'VND',
    FOREIGN KEY (advertiser_id) REFERENCES Advertiser(advertiser_id),
	FOREIGN KEY (currency_code) REFERENCES Currency(currency_code)
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
	currency_code NVARCHAR(10) NOT NULL DEFAULT 'VND',
    FOREIGN KEY (promote_id) REFERENCES Promote(promote_id),
    FOREIGN KEY (campaign_conversion_type_id) REFERENCES CampaignConversionType(campaign_conversion_id),
	FOREIGN KEY (currency_code) REFERENCES Currency(currency_code)
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
	currency_code NVARCHAR(10) NOT NULL DEFAULT 'VND',
    FOREIGN KEY (publisher_id) REFERENCES Publisher(publisher_id),
    FOREIGN KEY (campaign_id) REFERENCES Campaign(campaign_id),
	FOREIGN KEY (currency_code) REFERENCES Currency(currency_code)
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
	currency_code NVARCHAR(10) NOT NULL DEFAULT 'VND',	
    FOREIGN KEY (fraud_case_id) REFERENCES FraudCase(case_id),
	FOREIGN KEY (currency_code) REFERENCES Currency(currency_code)
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









-- Insert Currencies (VND as default)
INSERT INTO Currency (currency_code, currency_name, exchange_rate) VALUES
('VND', 'Vietnamese Dong', 1.000000);

-- Insert Admin
INSERT INTO [Admin] (username, email, password_hash, full_name, [role])
VALUES ('admin_user', 'admin@example.com', HASHBYTES('SHA2_256', 'SecurePass123'), 'System Administrator', 1);

-- Insert Publisher
INSERT INTO Publisher (username, email, password_hash, company_name, contact_name, phone_number, address, tax_id, registration_date, is_active, referral_code, [role])
VALUES ('pub_001', 'publisher1@example.com', HASHBYTES('SHA2_256', 'PublisherPass123'), 'Affiliate Network Ltd.', 'John Doe', '+84-123-456-789', '123 Affiliate St, HCMC, Vietnam', 'VN12345678', GETDATE(), 1, NEWID(), 2);

-- Insert Publisher Balance (VND)
INSERT INTO PublisherBalance (publisher_id, available_balance, pending_balance, lifetime_earnings, last_updated, currency_code)
VALUES (1, 12000000.00, 2400000.00, 50000000.00, GETDATE(), 'VND');

-- Insert Advertiser
INSERT INTO Advertiser (company_name, contact_name, email, password_hash, phone_number, address, website, industry, tax_id, registration_date, is_active, [role])
VALUES ('Tech Innovators Vietnam', 'Alice Nguyen', 'advertiser@example.vn', HASHBYTES('SHA2_256', 'AdvertiserPass123'), '+84-987-654-321', '456 Digital Ave, Hanoi, Vietnam', 'https://tech-innovators.vn', 'Technology', 'VN98765432', GETDATE(), 1, 3);

-- Insert Advertiser Balance (VND)
INSERT INTO AdvertiserBalance (advertiser_id, available_balance, pending_balance, lifetime_deposits, lifetime_withdrawals, lifetime_spend, last_updated, currency_code)
VALUES (1, 500000000.00, 100000000.00, 2500000000.00, 500000000.00, 2000000000.00, GETDATE(), 'VND');

-- Insert Campaign (VND)
INSERT INTO Campaign (advertiser_id, name, description, budget, daily_cap, monthly_cap, start_date, end_date, targeting_countries, targeting_devices, status, created_date, last_updated, is_private, conversion_rate, currency_code)
VALUES (1, 'Tết Sale Campaign', 'Promotion for Vietnamese Lunar New Year', 100000000.00, 5000000.00, 150000000.00, GETDATE(), DATEADD(DAY, 30, GETDATE()), 'VN', 'Mobile, Desktop', 'Active', GETDATE(), GETDATE(), 0, 2.5, 'VND');

-- Insert Traffic Source
INSERT INTO TrafficSource (publisher_id, name, type, url, added_date, is_active)
VALUES (1, 'Facebook Ads', 'Social Media', 'https://facebook.com/ad_campaign', GETDATE(), 1);

-- Insert Promote (Publisher promoting campaign)
INSERT INTO Promote (publisher_id, campaign_id, campaign_advertiser_url_id, base_tracking_url, join_date, is_approved, status, last_updated)
VALUES (1, 1, NULL, 'https://track.example.com/click?cid=1&pid=1', GETDATE(), 1, 'Active', GETDATE());

-- Insert Conversion Type
INSERT INTO ConversionType (name, description, tracking_method, requires_approval, action_type)
VALUES ('Purchase', 'Conversion when a sale is completed', 'Postback URL', 1, 'Sale');

-- ✅ Insert CampaignConversionType (Fixes missing reference in Conversion)
INSERT INTO CampaignConversionType (campaign_id, conversion_type_id, commission_amount, commission_type, cookie_window_days)
VALUES (1, 1, 1200000.00, 'Fixed', 30);

-- ✅ Insert Conversion (Fixes missing reference in FraudCase)
INSERT INTO Conversion (promote_id, campaign_conversion_type_id, conversion_type, commission_amount, conversion_value, conversion_time, status, transaction_id, ip_address, user_agent, country, city, device_type, browser, referrer, is_unique, is_suspicious, is_fraud)
VALUES (1, 1, 'Purchase', 1200000.00, '5000000.00', GETDATE(), 'Approved', 'TXN123456', '192.168.1.1', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64)', 'Vietnam', 'Ho Chi Minh', 'Desktop', 'Chrome', 'https://google.com', 1, 0, 0);

-- Insert Payment Method
INSERT INTO PaymentMethod (type, name, description, is_active, added_date)
VALUES ('Bank Transfer', 'Wire Transfer', 'Standard bank wire transfer', 1, GETDATE());

-- Insert Payout Request (Publisher requesting payment) - VND
INSERT INTO PayoutRequest (publisher_id, amount, request_date, status, reviewed_by, currency_code)
VALUES (1, 9600000.00, GETDATE(), 'Pending', NULL, 'VND');

-- Insert Deposit Request (Advertiser depositing funds) - VND
INSERT INTO DepositRequest (advertiser_id, amount, request_date, status, payment_method, transaction_id, currency_code)
VALUES (1, 200000000.00, GETDATE(), 'Completed', 'Bank Transfer', 'TXN789123', 'VND');

-- Insert Withdrawal Request (Advertiser withdrawing funds) - VND
INSERT INTO WithdrawalRequest (advertiser_id, amount, request_date, status, reviewed_by, currency_code)
VALUES (1, 80000000.00, GETDATE(), 'Approved', 1, 'VND');

-- Insert Fraud Type
INSERT INTO FraudType (name, description, severity_level)
VALUES ('Click Fraud', 'Fraudulent clicks detected from bot activity', 3);

-- ✅ Insert FraudCase (Fixes missing reference in FraudAdjustment)
INSERT INTO FraudCase (conversion_id, fraud_type_id, detection_date, evidence, status, resolution, resolution_date, detected_by, resolved_by)
VALUES (1, 1, GETDATE(), 'Suspicious IP with high click-through rate', 'Under Review', NULL, NULL, 1, NULL);

-- ✅ Insert FraudAdjustment (Now has a valid `fraud_case_id`)
INSERT INTO FraudAdjustment (fraud_case_id, original_amount, adjusted_amount, adjustment_percentage, adjustment_date, reason, approved_by, currency_code)
VALUES (1, 1200000.00, 0.00, 100.00, GETDATE(), 'Click fraud detected, commission revoked', 1, 'VND');








-- THIS IS FOR DELETE ALL TABLES
-- Step 1: Disable and Drop All Foreign Key Constraints
DECLARE @sql NVARCHAR(MAX) = '';

SELECT @sql += 'ALTER TABLE [' + TABLE_SCHEMA + '].[' + TABLE_NAME + '] DROP CONSTRAINT [' + CONSTRAINT_NAME + '];' + CHAR(10)
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE CONSTRAINT_TYPE = 'FOREIGN KEY';

PRINT 'Dropping Foreign Keys...';
EXEC sp_executesql @sql;

-- Step 2: Drop All Tables
SET @sql = '';

SELECT @sql += 'DROP TABLE IF EXISTS [' + TABLE_SCHEMA + '].[' + TABLE_NAME + '];' + CHAR(10)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';

PRINT 'Dropping Tables...';
EXEC sp_executesql @sql;

-- Optional: Step 3 - Drop Views, if needed
SET @sql = '';

SELECT @sql += 'DROP VIEW IF EXISTS [' + TABLE_SCHEMA + '].[' + TABLE_NAME + '];' + CHAR(10)
FROM INFORMATION_SCHEMA.VIEWS;

PRINT 'Dropping Views...';
EXEC sp_executesql @sql;

-- Step 4: Re-enable Foreign Keys (Optional, for future use)
PRINT 'All tables dropped successfully!';







-- THIS IS FOR DELETE ALL ROWS FROM ALL TABLES - NOT WORKING
-- Step 1: Disable Foreign Key Constraints
DECLARE @sql NVARCHAR(MAX) = '';

SELECT @sql += 'ALTER TABLE [' + TABLE_SCHEMA + '].[' + TABLE_NAME + '] NOCHECK CONSTRAINT ALL;' + CHAR(10)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';

PRINT 'Disabling Foreign Keys...';
EXEC sp_executesql @sql;

-- Step 2: Delete All Data from Tables
SET @sql = '';

SELECT @sql += 'DELETE FROM [' + TABLE_SCHEMA + '].[' + TABLE_NAME + '];' + CHAR(10)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';

PRINT 'Deleting All Data...';
EXEC sp_executesql @sql;

-- Step 3: Reset Identity Columns (Restart IDs from 1)
SET @sql = '';

SELECT @sql += 'DBCC CHECKIDENT ([' + TABLE_SCHEMA + '].[' + TABLE_NAME + '], RESEED, 0);' + CHAR(10)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';

PRINT 'Resetting Identity Columns...';
EXEC sp_executesql @sql;

-- Step 4: Re-enable Foreign Key Constraints
SET @sql = '';

SELECT @sql += 'ALTER TABLE [' + TABLE_SCHEMA + '].[' + TABLE_NAME + '] CHECK CONSTRAINT ALL;' + CHAR(10)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';

PRINT 'Re-enabling Foreign Keys...';
EXEC sp_executesql @sql;

PRINT 'All rows deleted and identity columns reset!';
