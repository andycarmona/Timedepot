USE [timessence]
GO

/****** Object:  Table [dbo].[Payments]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Payments](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[PaymentNo] [nvarchar](MAX) NULL,
	[CustomerNo] [nvarchar](MAX) NULL,
	[SalesOrderNo] [nvarchar](MAX) NULL,
	[PaymentType] [nvarchar](MAX) NULL,
	[CreditCardNumber] [nvarchar](MAX) NULL,
	[ReferenceNo] [nvarchar](MAX) NULL,
	[Amount] [money] NULL,
	[PaymentDate] [DateTime] NULL,
) 

GO

SET ANSI_PADDING OFF
GO


