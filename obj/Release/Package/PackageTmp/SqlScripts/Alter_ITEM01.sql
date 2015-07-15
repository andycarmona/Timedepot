USE [timessence]
GO
ALTER TABLE dbo.ITEM ALTER COLUMN UnitPerCase nvarchar(MAX);
GO
ALTER TABLE dbo.ITEM ALTER COLUMN UnitWeight nvarchar(MAX);
GO
ALTER TABLE dbo.ITEM ALTER COLUMN CaseWeight nvarchar(MAX);
GO
