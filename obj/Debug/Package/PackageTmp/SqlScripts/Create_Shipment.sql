USE [officetestdata]
GO

/****** Object:  Table [dbo].[Shipment]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Shipment](
	[ShipmentId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[ShipmentDate] [DateTime] NULL,
	[InvoiceId] [int] NULL,
	[InvoiceNo] [nvarchar](MAX) NULL,
	[RateResults] [nvarchar](MAX) NULL,
) 

GO

SET ANSI_PADDING OFF
GO


