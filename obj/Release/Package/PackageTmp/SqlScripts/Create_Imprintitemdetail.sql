USE [asidata2013]
GO

/****** Object:  Table [dbo].[Imprintitemdetail]    Script Date: 22/06/2013 9:57:53 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Imprintitemdetail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[printId] [int] NULL,
	[itemId] [varchar](30) NULL,
	[RowNo] [int] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


