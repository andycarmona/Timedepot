USE [timessence]
GO
ALTER TABLE dbo.SalesOrderDetail ADD ItemPosition int DEFAULT 0 ;
GO
ALTER TABLE dbo.SalesOrderDetail ADD ItemOrder float DEFAULT 0 ;
GO
