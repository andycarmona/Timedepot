USE [timessence]
GO

/****** Object:  Table [dbo].[InitialInfo]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[InitialInfo](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[SalesOrderNo] [int] NOT NULL DEFAULT 0,
	[InvoiceNo] [int]  NOT NULL DEFAULT 0,
	[PaymentNo] [int]  NOT NULL DEFAULT 0,
	[PurchaseOrderNo] [int]  NOT NULL DEFAULT 0,
	[TaxRate] [float]  NOT NULL DEFAULT 0,
	[PaymentAccount] [nvarchar](MAX) NULL,
	[ShipperInfo] [nvarchar](MAX) NULL,
) 

GO

SET ANSI_PADDING OFF
GO


