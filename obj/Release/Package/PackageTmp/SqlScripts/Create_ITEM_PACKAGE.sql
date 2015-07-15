USE [asidata2013]
GO

/****** Object:  Table [dbo].[ITEM_PACKAGE]    Script Date: 21/06/2013 7:24:36 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ITEM_PACKAGE](
	[Item] [nvarchar](50) NULL,
	[Package] [nvarchar](50) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO


