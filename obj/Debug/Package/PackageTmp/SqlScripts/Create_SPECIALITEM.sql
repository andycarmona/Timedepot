USE [asidata2013]
GO

/****** Object:  Table [dbo].[SPECIALITEM]    Script Date: 20/06/2013 8:48:30 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SPECIALITEM](
	[ItemID] [nvarchar](50) NULL,
	[Price] [nvarchar](50) NULL,
	[Option] [nvarchar](max) NULL,
	[PicID] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


