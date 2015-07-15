USE [asidata2013]
GO

/****** Object:  Table [dbo].[UserRegistration]    Script Date: 20/06/2013 4:12:34 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UserRegistration](
	[RId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](200) NULL,
	[UserPassword] [varchar](200) NULL,
	[Date] [datetime] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


