USE [timessence]
GO

/****** Object:  Table [dbo].[PackageRateLogParameters]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PackageRateLogParameters](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[IdRateLog] [int] NULL,
	[Parameter] [nvarchar](MAX) NULL,
	[ParameterValue] [nvarchar](MAX) NULL,
) 

GO

SET ANSI_PADDING OFF
GO


