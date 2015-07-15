USE [timessence]
GO

/****** Object:  Table [dbo].[timelydepot_log]    Script Date: 20/06/2013 7:51:02 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[timelydepot_log](
	[logID] [int] IDENTITY(1,1) NOT NULL,
	[logTime] [smalldatetime] NOT NULL,
	[event] [varchar](255) NOT NULL,
	[note] [varchar](max) NOT NULL,
 CONSTRAINT [PK_timelydepot_log] PRIMARY KEY CLUSTERED 
(
	[logID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


