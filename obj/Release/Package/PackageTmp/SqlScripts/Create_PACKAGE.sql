USE [asidata2013]
GO

/****** Object:  Table [dbo].[PACKAGE]    Script Date: 21/06/2013 6:30:59 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PACKAGE](
	[PackageID] [nvarchar](50) NULL,
	[Weight] [nvarchar](50) NULL,
	[Price] [smallmoney] NULL,
	[Description] [nvarchar](max) NULL,
	[ImagePath] [varchar](500) NULL,
	[DiscountCode] [varchar](500) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


