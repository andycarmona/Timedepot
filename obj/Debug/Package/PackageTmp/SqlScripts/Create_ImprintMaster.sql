USE [asidata2013]
GO

/****** Object:  Table [dbo].[ImprintMaster]    Script Date: 22/06/2013 9:41:48 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ImprintMaster](
	[ImprintId] [int] IDENTITY(1,1) NOT NULL,
	[ImprintName] [varchar](100) NULL,
	[SetUpCharge] [decimal](18, 2) NULL,
	[SetUpCharge2] [decimal](18, 2) NULL,
	[RunCharge] [decimal](18, 2) NULL,
	[ColorCharge] [decimal](18, 2) NULL,
	[Displayname] [varchar](60) NULL,
	[DiscountCode] [varchar](60) NULL,
	[ImagePath] [varchar](60) NULL,
	[NumberColor] [int] NULL,
	[Information] [varchar](500) NULL,
	[RunChargeInclude] [varchar](60) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


