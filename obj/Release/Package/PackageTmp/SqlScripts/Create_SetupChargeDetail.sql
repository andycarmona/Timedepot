USE [asidata2013]
GO

/****** Object:  Table [dbo].[SetupChargeDetail]    Script Date: 20/06/2013 9:00:37 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SetupChargeDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SetUpCharge] [decimal](18, 2) NULL,
	[RunCharge] [decimal](18, 2) NULL,
	[itemid] [varchar](40) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


