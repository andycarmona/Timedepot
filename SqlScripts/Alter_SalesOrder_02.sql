USE [timessence]
GO
ALTER TABLE dbo.SalesOrder ADD Approvedby nvarchar(MAX) ;
GO
ALTER TABLE dbo.SalesOrder ADD AprovedDate [DateTime] NULL ;
GO
ALTER TABLE dbo.SalesOrder ADD Requiredate [DateTime] NULL ;
GO
