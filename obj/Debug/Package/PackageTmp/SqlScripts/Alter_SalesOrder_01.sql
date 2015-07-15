USE [timessence]
GO
ALTER TABLE dbo.SalesOrder ADD Tax_rate decimal(21,6) null ;
GO
ALTER TABLE dbo.SalesOrder ADD Invs_Tax decimal(21,6) null ;
GO
