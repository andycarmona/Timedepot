USE [asidata2013]
GO

/****** Object:  Table [dbo].[UserQuotation]    Script Date: 20/06/2013 4:37:34 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UserQuotation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [varchar](20) NULL,
	[UserId] [int] NULL,
	[Date] [datetime] NULL,
	[PostCode] [varchar](8) NULL,
	[PostalType] [varchar](10) NULL,
	[ImprintType] [varchar](12) NULL,
	[Status] [int] NULL,
	[Invoicestatus] [int] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


