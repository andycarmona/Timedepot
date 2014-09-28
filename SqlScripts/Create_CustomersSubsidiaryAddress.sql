USE [timessence]
GO

/****** Object:  Table [dbo].[CustomersSubsidiaryAddress]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CustomersSubsidiaryAddress](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[CustomerId] [int] NULL,
	[CompanyName] [nvarchar](MAX) NULL,
	[FirstName] [nvarchar](MAX) NULL,
	[LastName] [nvarchar](MAX) NULL,
	[Title] [nvarchar](MAX) NULL,
	[Address] [nvarchar](MAX) NULL,
	[City] [nvarchar](MAX) NULL,
	[State] [nvarchar](MAX) NULL,
	[Zip] [nvarchar](MAX) NULL,
	[Country] [nvarchar](MAX) NULL,
	[Tel] [nvarchar](MAX) NULL,
	[Fax] [nvarchar](MAX) NULL,
	[Email] [nvarchar](MAX) NULL,
	[Website] [nvarchar](MAX) NULL,
	[Note] [nvarchar](MAX) NULL,
) 

GO

SET ANSI_PADDING OFF
GO


