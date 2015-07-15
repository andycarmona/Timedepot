USE [asidata2013]
GO

/****** Object:  Table [dbo].[COLLECTION]    Script Date: 22/06/2013 5:12:55 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COLLECTION](
	[ColID] [nvarchar](50) NULL,
	[CatID] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


