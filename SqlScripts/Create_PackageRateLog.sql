USE [timessence]
GO

/****** Object:  Table [dbo].[PackageRateLog]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PackageRateLog](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[DateSubmit] [DateTime] NULL,
	[ItemId] [nvarchar](MAX) NULL,
	[Quantity] [nvarchar](MAX) NULL,
	[PostalZipCode] [nvarchar](MAX) NULL,
	[Boxes] [nvarchar](MAX) NULL,
	[ItemsLastBox] [nvarchar](MAX) NULL,
	[FullBoxWeight] [nvarchar](MAX) NULL,
	[PartialBoxWeight] [nvarchar](MAX) NULL,
	[ValueperFullBox] [nvarchar](MAX) NULL,
	[ValueperPartialBox] [nvarchar](MAX) NULL,
	[CaseHeight] [nvarchar](MAX) NULL,
	[CaseLenght] [nvarchar](MAX) NULL,
	[CaseWidth] [nvarchar](MAX) NULL,
	[CaseWeight] [nvarchar](MAX) NULL,
	[UnitPrice] [nvarchar](MAX) NULL,
) 

GO

SET ANSI_PADDING OFF
GO


