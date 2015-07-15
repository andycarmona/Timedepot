USE [timessence]
GO

/****** Object:  Table [dbo].[SalesOrderTES]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SalesOrderTES](
	[SalesOrderId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[SalesOrderNo] [nvarchar](MAX) NULL,
	[SODate] [DateTime] NULL,
	[ShipVia] [nvarchar](MAX) NULL,
	[ShipDate] [DateTime] NULL,
	[TradeId] [int] NULL,
	[CustomerId] [int] NULL,
	[CustomerShiptoId] [int] NULL,
	[CustomerShipLocation] [nvarchar](MAX) NULL,
	[VendorId] [int] NULL,
	[BussinesType] [nvarchar](MAX) NULL,
	[VendorAddress] [nvarchar](MAX) NULL,
	[PurchaseOrderNo] [nvarchar](MAX) NULL,
	[PaymentTerms] [nvarchar](MAX) NULL,
	[PaymentDate] [DateTime] NULL,
	[PaymentAmount] [money] NULL,
	[ShippingHandling] [money] NULL,
	[CreaditCardNo] [nvarchar](MAX) NULL,
	[Note] [nvarchar](MAX) NULL,
	[IsBlindShip] [bit] NULL DEFAULT 0,
	[SalesRep] [nvarchar](MAX) NULL,
	[Tax_rate] [float] NULL DEFAULT 0,
	[Invs_Tax] [float] NULL DEFAULT 0,
	[Approvedby] [nvarchar](MAX) NULL,
	[AprovedDate] [DateTime] NULL,
	[Requiredate] [DateTime] NULL
) 

GO

SET ANSI_PADDING OFF
GO


