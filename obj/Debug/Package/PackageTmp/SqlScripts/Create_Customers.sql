USE [timessence]
GO

/****** Object:  Table [dbo].[Customers]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Customers](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[CustomerNo] [nvarchar](MAX) NULL,
	[Status] [bit] NOT NULL DEFAULT 0,
	[BussinesType] [nvarchar](MAX) NULL,
	[SalesPerson] [nvarchar](MAX) NULL,
	[DeptoNo] [nvarchar](MAX) NULL,
	[SellerPermintNo] [nvarchar](MAX) NULL,
	[ASINo] [nvarchar](MAX) NULL,
	[PPAINo] [nvarchar](MAX) NULL,
	[SageNo] [nvarchar](MAX) NULL,
	[Origin] [nvarchar](MAX) NULL,
	[CreditLimit] [money] NULL,
	[PaymentTerms] [nvarchar](MAX) NULL,
	[BussinesSice] [DateTime] NULL,
) 

GO

SET ANSI_PADDING OFF
GO


