USE [asidata2013]
GO

/****** Object:  Table [dbo].[UserQuotationDetail]    Script Date: 20/06/2013 4:48:43 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UserQuotationDetail](
	[DetailId] [int] NULL,
	[ProductType] [varchar](100) NULL,
	[Quantity] [varchar](20) NULL,
	[Amount] [decimal](18, 2) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


