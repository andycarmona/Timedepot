USE [asidata2013]
GO

/****** Object:  Table [dbo].[InformationId]    Script Date: 21/06/2013 8:08:46 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[InformationId](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InformationId1] [varchar](20) NULL,
	[Description] [varchar](800) NULL,
	[Priceinformation] [varchar](200) NULL,
	[ProductionTime] [varchar](max) NULL,
	[Artwork] [varchar](200) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


