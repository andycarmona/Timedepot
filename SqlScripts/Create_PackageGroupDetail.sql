USE [asidata2013]
GO

/****** Object:  Table [dbo].[PackageGroupDetail]    Script Date: 21/06/2013 6:18:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PackageGroupDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [varchar](100) NULL,
	[PAckageId] [varchar](100) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


