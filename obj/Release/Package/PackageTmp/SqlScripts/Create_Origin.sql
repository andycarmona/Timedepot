USE [timessence]
GO

/****** Object:  Table [dbo].[Origin]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Origin](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Name] [varchar](MAX) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO