USE [asidata2013]
GO

/****** Object:  Table [dbo].[ITEM]    Script Date: 21/06/2013 7:42:57 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ITEM](
	[ItemID] [varchar](50) NULL,
	[PicID] [varchar](50) NULL,
	[DialType] [varchar](50) NULL,
	[DescA] [varchar](max) NULL,
	[DescB] [varchar](max) NULL,
	[CollectionID] [varchar](50) NULL,
	[Price_ID] [nvarchar](50) NULL,
	[Misc_ID] [nvarchar](50) NULL,
	[Inactive] [nvarchar](1) NULL,
	[Keywords] [nvarchar](max) NULL,
	[Special] [nvarchar](1) NULL,
	[New] [nvarchar](1) NULL,
	[title] [varchar](600) NULL,
	[UnitPerCase] [int] NULL,
	[UnitWeight] [int] NULL,
	[CaseWeight] [int] NULL,
	[DimensionH] [int] NULL,
	[DimensionL] [int] NULL,
	[DimensionD] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


