USE [timessence]
GO

/****** Object:  Table [dbo].[CustomersCreditCardShipping]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CustomersCreditCardShipping](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[CustomerId] [int]  NULL,
	[Name] [nvarchar](MAX) NULL,
	[CreditNumber] [nvarchar](MAX) NULL,
	[CardType] [nvarchar](MAX) NULL,
	[SecureCode] [nvarchar](MAX) NULL,
	[ExpirationDate] [datetime] NULL,
	[Tel] [nvarchar](MAX) NULL,
	[Address1] [nvarchar](MAX) NULL,
	[Address2] [nvarchar](MAX) NULL,
	[City] [nvarchar](MAX) NULL,
	[State] [nvarchar](MAX) NULL,
	[Zip] [nvarchar](MAX) NULL,
	[Country] [nvarchar](MAX) NULL,
) 

GO

SET ANSI_PADDING OFF
GO


