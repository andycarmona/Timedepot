USE [timessence]
GO

/****** Object:  Table [dbo].[webpages_Roles]    Script Date: 29/06/2013 8:00:39 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[webpages_Roles](
	[RoleId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
) ON [PRIMARY]

GO


