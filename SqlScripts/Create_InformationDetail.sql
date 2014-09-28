USE [asidata2013]
GO

/****** Object:  Table [dbo].[InformationDetail]    Script Date: 21/06/2013 8:23:37 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[InformationDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InformationId] [int] NULL,
	[ItemId] [varchar](20) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


