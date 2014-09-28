USE [officetestdata]
GO

/****** Object:  Table [dbo].[ShipmentDetails]    Script Date: 21/06/2013 7:42:57 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ShipmentDetails](
	[ShipmentId] [int] NULL,
	[ShipmentDetailID]  [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[DetailId] [int] NULL,
	[BoxNo] [nvarchar](max) NULL,
	[Sub_ItemID] [nvarchar](max) NULL,
	[Quantity] [float] NULL,
	[UnitPrice] [money] NULL,
	[UnitWeight] [int] NULL,
	[DimensionH] [int] NULL,
	[DimensionL] [int] NULL,
	[DimensionD] [int] NULL,
	[Reference1] [nvarchar](max) NULL,
	[Reference2] [nvarchar](max) NULL,
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


