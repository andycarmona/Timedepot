USE [asidata2013]
GO

/****** Object:  Table [dbo].[PackagingDetail]    Script Date: 21/06/2013 3:05:56 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PackagingDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PackId] [int] NULL,
	[ItemNo] [varchar](100) NULL,
	[DisplayName] [varchar](100) NULL,
	[Price] [decimal](18, 2) NULL,
	[ImagePath] [varchar](60) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


