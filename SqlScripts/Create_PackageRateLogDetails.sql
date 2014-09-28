USE [timessence]
GO

/****** Object:  Table [dbo].[PackageRateLogDetails]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PackageRateLogDetails](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[IdRateLog] [int] NULL,
	[BoxNo] [nvarchar](MAX) NULL,
	[Quantity] [nvarchar](MAX) NULL,
	[Dimensions] [nvarchar](MAX) NULL,
	[DimensionsUnits] [nvarchar](MAX) NULL,
	[WeigthUnits] [nvarchar](MAX) NULL,
	[DeclaredValue] [nvarchar](MAX) NULL,
	[RequestCode] [nvarchar](MAX) NULL,
	[PackageTypeCode] [nvarchar](MAX) NULL,
) 

GO

SET ANSI_PADDING OFF
GO


