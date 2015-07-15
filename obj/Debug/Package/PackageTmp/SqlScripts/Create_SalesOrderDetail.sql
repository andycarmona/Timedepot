USE [timessence]
GO

/****** Object:  Table [dbo].[SalesOrderDetail]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SalesOrderDetail](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[SalesOrderId] [int] NULL,
	[Quantity] [float] NULL,
	[ShipQuantity] [float] NULL,
	[BackOrderQuantity] [float] NULL,
	[ItemID] [nvarchar](MAX) NULL,
	[Sub_ItemID] [nvarchar](MAX) NULL,
	[Description] [nvarchar](MAX) NULL,
	[Tax] [float] NULL,
	[UnitPrice] [money] NULL,
) 

GO

SET ANSI_PADDING OFF
GO


