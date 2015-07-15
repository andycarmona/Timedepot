USE [timessence]
GO
ALTER TABLE dbo.SalesOrder ADD QuoteId [int] NULL ;
GO
ALTER TABLE dbo.SalesOrder ADD FromName nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD FromTitle nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD FromCompany nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD FromAddress1 nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD FromAddress2 nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD FromCity nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD FromState nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD FromZip nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD FromCountry nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD FromEmail nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD FromTel nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD FromFax nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD ToName nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD ToTitle nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD ToCompany nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD ToAddress1 nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD ToAddress2 nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD ToCity nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD ToState nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD ToZip nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD ToCountry nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD ToEmail nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD ToTel nvarchar(MAX);
GO
ALTER TABLE dbo.SalesOrder ADD ToFax nvarchar(MAX);
GO
