USE [timessence]
GO

/****** Object:  Table [dbo].[Setup for Price]    Script Date: 21/06/2013 7:16:19 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Setup_for_Price](
	[Item] [nvarchar](50) NULL,
	[Qty] [smallint] NULL,
	[Price] [smallmoney] NULL,
	[Discount Code] [nvarchar](50) NULL,
	[Description] [nvarchar](20) NULL,
	[Id] [int] NOT NULL
) ON [PRIMARY]

GO


