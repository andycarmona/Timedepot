USE [asidata2013]
GO

/****** Object:  Table [dbo].[SETUP]    Script Date: 21/06/2013 9:27:53 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SETUP](
	[SetupID] [nvarchar](50) NULL,
	[Run_charge] [int] NULL,
	[SetupCost] [int] NULL,
	[Discount_Code] [nvarchar](50) NULL,
	[Min] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[Dial] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


