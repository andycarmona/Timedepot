USE [timessence]
GO
ALTER TABLE dbo.UserQuotationDetail ADD Id int IDENTITY CONSTRAINT UserQuotationDetailId_pk PRIMARY KEY ;
GO
ALTER TABLE dbo.UserQuotationDetail ADD ItemID nvarchar(max);
GO
ALTER TABLE dbo.UserQuotationDetail ADD Status int NULL DEFAULT 0;
GO
ALTER TABLE dbo.UserQuotationDetail ADD ShippedQuantity float NULL DEFAULT 0;
GO
ALTER TABLE dbo.UserQuotationDetail ADD BOQuantity float NULL DEFAULT 0;
GO
