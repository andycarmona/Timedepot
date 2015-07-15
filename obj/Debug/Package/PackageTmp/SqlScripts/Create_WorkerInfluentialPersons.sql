USE [asidata2013]
GO

/****** Object:  Table [dbo].[WorkerInfluentialPersons]    Script Date: 20/06/2013 6:47:53 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WorkerInfluentialPersons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[categoryID] [int]  NOT NULL,
	[influentailPersonID] [int] NULL
) ON [PRIMARY]

GO


