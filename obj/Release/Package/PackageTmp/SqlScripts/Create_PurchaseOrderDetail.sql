USE [timessence]
GO

/****** Object:  Table [dbo].[PurchasOrderDetail]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PurchasOrderDetail](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[PurchaseOrderId] [int] NULL,
	[ItemID] [nvarchar](MAX) NULL,
	[Sub_ItemID] [nvarchar](MAX) NULL,
	[Description] [nvarchar](MAX) NULL,
	[Quantity] [float] NULL,
	[Tax] [float] NULL,
	[UnitPrice] [money] NULL,
	[VendorReference] [nvarchar](MAX) NULL,
	[ItemPosition] [int] NULL,
	[ItemOrder] [float] NULL,
) 

GO

SET ANSI_PADDING OFF
GO


