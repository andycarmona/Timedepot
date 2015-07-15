USE [timessence]
GO

/****** Object:  Table [dbo].[Vendors]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Vendors](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[VendorNo] [nvarchar](MAX) NULL,
	[Status] [bit] NOT NULL DEFAULT 0,
	[BussinesType] [nvarchar](MAX) NULL,
	[Origin] [nvarchar](MAX) NULL,
	[CreditLimit] [money] NULL,
	[PaymentTerms] [nvarchar](MAX) NULL,
	[BussinesSice] [DateTime] NULL,
) 

GO

SET ANSI_PADDING OFF
GO


