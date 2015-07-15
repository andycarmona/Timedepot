USE [timessence]
GO
ALTER TABLE dbo.ITEM ADD Status bit DEFAULT 0 ;
GO
ALTER TABLE dbo.ITEM ADD Pic2ID nvarchar(MAX);
GO
ALTER TABLE dbo.ITEM ADD Pic3ID nvarchar(MAX);
GO
ALTER TABLE dbo.ITEM ADD DeptoNo nvarchar(MAX);
GO
ALTER TABLE dbo.ITEM ADD YearProduct nvarchar(MAX);
GO
ALTER TABLE dbo.ITEM ADD ClassNo nvarchar(MAX);
GO
ALTER TABLE dbo.ITEM ADD UPCCode nvarchar(MAX);
GO
ALTER TABLE dbo.ITEM ADD CaseDimensionL nvarchar(MAX);
GO
ALTER TABLE dbo.ITEM ADD CaseDimensionW nvarchar(MAX);
GO
ALTER TABLE dbo.ITEM ADD CaseDimensionH nvarchar(MAX);
GO
ALTER TABLE dbo.ITEM ADD Note nvarchar(MAX);
GO