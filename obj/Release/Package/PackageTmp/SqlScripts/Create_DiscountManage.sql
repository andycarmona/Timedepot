USE [asidata2013]
GO

/****** Object:  Table [dbo].[DiscountManage]    Script Date: 22/06/2013 3:37:47 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[DiscountManage](
	[DiscountName] [varchar](60) NULL,
	[DiscountPercentage] [varchar](60) NULL,
	[id] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


