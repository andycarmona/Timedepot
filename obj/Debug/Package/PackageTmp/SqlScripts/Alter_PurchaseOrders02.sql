USE [timessence]
GO
ALTER TABLE dbo.PurchaseOrders ADD Invoice nvarchar(MAX);
GO
ALTER TABLE dbo.PurchaseOrders ADD TrackingNo nvarchar(MAX);
GO
ALTER TABLE dbo.PurchaseOrders ADD Terms nvarchar(MAX);
GO
ALTER TABLE dbo.PurchaseOrders ADD OrderBy nvarchar(MAX);
GO
ALTER TABLE dbo.PurchaseOrders ADD Warehouse nvarchar(MAX);
GO
ALTER TABLE dbo.PurchaseOrders ADD Billto nvarchar(MAX);
GO
ALTER TABLE dbo.PurchaseOrders ADD ShipVia nvarchar(MAX);
GO
