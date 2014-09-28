USE [timessence]
GO
ALTER TABLE dbo.SetupChargeDetail ADD ReSetupCharge decimal(18,2);
GO
ALTER TABLE dbo.SetupChargeDetail ADD ReSetupChargeDiscountCode nvarchar(MAX);
GO
ALTER TABLE dbo.SetupChargeDetail ADD RunChargeDiscountCode nvarchar(MAX);
GO
ALTER TABLE dbo.SetupChargeDetail ADD SetupChargeDiscountCode nvarchar(MAX);
GO
ALTER TABLE dbo.SetupChargeDetail ADD FirstSetupFree bit DEFAULT 0;
GO

