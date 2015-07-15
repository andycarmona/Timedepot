USE [timessence]
GO

/****** Object:  Table [dbo].[Trade]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Trade](
	[TradeId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[TradeName] [nvarchar](MAX) NULL,
	[Address] [nvarchar](MAX) NULL,
	[City] [nvarchar](MAX) NULL,
	[State] [nvarchar](MAX) NULL,
	[PostCode] [nvarchar](MAX) NULL,
	[Country] [nvarchar](MAX) NULL,
	[Tel] [nvarchar](MAX) NULL,
	[Fax] [nvarchar](MAX) NULL,
	[WebSite] [nvarchar](MAX) NULL,
	[Email] [nvarchar](MAX) NULL,
) 

GO

SET ANSI_PADDING OFF
GO


