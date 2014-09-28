USE [timessence]
GO
ALTER TABLE dbo.Invoice ADD FromName nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD FromTitle nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD FromCompany nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD FromAddress1 nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD FromAddress2 nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD FromCity nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD FromState nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD FromZip nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD FromCountry nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD FromEmail nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD FromTel nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD FromFax nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD ToName nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD ToTitle nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD ToCompany nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD ToAddress1 nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD ToAddress2 nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD ToCity nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD ToState nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD ToZip nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD ToCountry nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD ToEmail nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD ToTel nvarchar(MAX);
GO
ALTER TABLE dbo.Invoice ADD ToFax nvarchar(MAX);
GO
