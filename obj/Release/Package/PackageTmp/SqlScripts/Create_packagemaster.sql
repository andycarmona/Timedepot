USE [asidata2013]
GO

/****** Object:  Table [dbo].[packagemaster]    Script Date: 21/06/2013 3:35:17 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[packagemaster](
	[PackId] [int] IDENTITY(1,1) NOT NULL,
	[PackName] [varchar](50) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


