USE [asidata2013]
GO

/****** Object:  Table [dbo].[EventLog]    Script Date: 22/06/2013 12:46:19 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EventLog](
	[id] [int]  IDENTITY(1,1)  NOT NULL,
	[EventLog1] [nvarchar](50) NULL,
	[IP] [nvarchar](50) NULL,
	[User] [nvarchar](50) NULL,
	[EventTime] [datetime] NULL
) ON [PRIMARY]

GO


