USE [timessence]
GO
ALTER TABLE dbo.Invoice ADD Tax_rate decimal(21,6) null ;
GO
ALTER TABLE dbo.Invoice ADD Invs_Tax decimal(21,6) null ;
GO
