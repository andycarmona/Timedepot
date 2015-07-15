USE [asidata2013]
GO

/****** Object:  Table [dbo].[SUB_SPECIALITEM]    Script Date: 20/06/2013 8:10:52 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SUB_SPECIALITEM](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [nvarchar](50) NULL,
	[Option] [nvarchar](50) NULL,
	[OptionCost] [nvarchar](50) NULL
) ON [PRIMARY]

GO


