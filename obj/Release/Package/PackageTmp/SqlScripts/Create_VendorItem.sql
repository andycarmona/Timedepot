USE [timessence]
GO

/****** Object:  Table [dbo].[VendorItem]    Script Date: 22/06/2013 5:32:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[VendorItem](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[ItemId] [nvarchar](50) NULL,
	[VendorNo] [nvarchar](50) NULL,
	[VendorPartNo] [nvarchar](50) NULL,
	[Cost] [money] NULL,
	[CostBlind] [money] NULL,
	[RunCharge] [money] NULL,
	[SetupCharge] [money] NULL,
	[ReSetupCharge] [money] NULL,
	[LeadTimeHrs] [int] NULL,
	[LeadTimeMin] [int] NULL,
	[LeadTimeSec] [int] NULL,
	[LeadTime] [Time] NULL,
	[UpdateDate] [DateTime] NULL,
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


