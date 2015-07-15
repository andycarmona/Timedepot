USE [timessence]
GO

/****** Object:  Table [dbo].[Vendors]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[VendorsSpecialNotes](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[VendorId] [int]  NULL,
	[SpecialNote] [nvarchar](MAX) NULL,
) 

GO

SET ANSI_PADDING OFF
GO


