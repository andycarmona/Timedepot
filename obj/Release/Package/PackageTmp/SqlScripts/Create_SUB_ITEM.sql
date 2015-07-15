USE [asidata2013]
GO

/****** Object:  Table [dbo].[SUB_ITEM]    Script Date: 20/06/2013 8:26:04 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SUB_ITEM](
	[ItemID] [nvarchar](50) NULL,
	[Description] [nvarchar](50) NULL,
	[Field1] [nvarchar](50) NULL,
	[Sub_ItemID] [nvarchar](50) NULL,
	[Sub_GroupID] [nvarchar](100) NULL
) ON [PRIMARY]

GO


