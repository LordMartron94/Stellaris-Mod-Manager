CREATE TABLE [dbo].[Folder]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [DisplayPriority] INT NOT NULL 
)
