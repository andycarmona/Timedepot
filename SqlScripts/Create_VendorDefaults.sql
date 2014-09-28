USE [timessence]
GO

/****** Object:  Table [dbo].[VendorDefaults]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[VendorDefaults](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[VendorId] [int] NULL,
	[SalesContactId] [int] NULL,
	[SalesName] [nvarchar](MAX) NULL,
	[SubsidiaryId] [int] NULL,
	[SubsidiaryName] [nvarchar](MAX) NULL,
	[ShiptoAddressId] [int] NULL,
	[ShiptoName] [nvarchar](MAX) NULL,
	[NoteId] [int] NULL,
	[NoteName] [nvarchar](MAX) NULL,
) 

GO

SET ANSI_PADDING OFF
GO


