USE [timessence]
GO

/****** Object:  Table [dbo].[PurchaseOrders]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PurchaseOrders](
	[PurchaseOrderId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[PurchaseOrderNo] [nvarchar](MAX) NULL,
	[SalesOrderNo] [nvarchar](MAX) NULL,
	[TradeId] [int] NULL,
	[PODate] [DateTime] NULL,
	[VendorId]  [nvarchar](MAX) NULL,
	[ShipDate] [DateTime] NULL,
	[PaidBy] [nvarchar](MAX) NULL,
	[BlindDrop] [nvarchar](MAX) NULL,
	[Logo] [nvarchar](MAX) NULL,
	[ImprintColor] [nvarchar](MAX) NULL,
	[PurchaseOrderReference] [nvarchar](MAX) NULL,
	[IsBlindShip] [bit] NULL DEFAULT 0,
	[FromName] [nvarchar](MAX) NULL,
	[FromTitle] [nvarchar](MAX) NULL,
	[FromCompany] [nvarchar](MAX) NULL,
	[FromAddress1] [nvarchar](MAX) NULL,
	[FromAddress2] [nvarchar](MAX) NULL,
	[FromCity] [nvarchar](MAX) NULL,
	[FromState] [nvarchar](MAX) NULL,
	[FromZip] [nvarchar](MAX) NULL,
	[FromCountry] [nvarchar](MAX) NULL,
	[FromEmail] [nvarchar](MAX) NULL,
	[FromTel] [nvarchar](MAX) NULL,
	[FromFax] [nvarchar](MAX) NULL,
	[ToName] [nvarchar](MAX) NULL,
	[ToTitle] [nvarchar](MAX) NULL,
	[ToCompany] [nvarchar](MAX) NULL,
	[ToAddress1] [nvarchar](MAX) NULL,
	[ToAddress2] [nvarchar](MAX) NULL,
	[ToCity] [nvarchar](MAX) NULL,
	[ToState] [nvarchar](MAX) NULL,
	[ToZip] [nvarchar](MAX) NULL,
	[ToCountry] [nvarchar](MAX) NULL,
	[ToEmail] [nvarchar](MAX) NULL,
	[ToTel] [nvarchar](MAX) NULL,
	[ToFax] [nvarchar](MAX) NULL,
) 

GO

SET ANSI_PADDING OFF
GO


