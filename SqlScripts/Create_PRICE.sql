USE [asidata2013]
GO

/****** Object:  Table [dbo].[PRICE]    Script Date: 21/06/2013 9:47:30 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRICE](
	[Item] [nvarchar](50) NULL,
	[Qty] [smallint] NULL,
	[PriceType] [nvarchar](50) NULL,
	[thePrice] [smallmoney] NULL,
	[Discount_Code] [nvarchar](50) NULL,
	[Description] [nvarchar](20) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO


