USE [timessence]
GO

/****** Object:  Table [dbo].[UserProfile]    Script Date: 29/06/2013 7:57:00 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserProfile](
	[UserId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[UserName] [nvarchar](56) NOT NULL,
) 

GO


