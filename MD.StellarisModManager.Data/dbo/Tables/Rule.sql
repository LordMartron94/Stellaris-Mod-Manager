CREATE TABLE [dbo].[Rule]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AssociatedModId] INT NOT NULL, 
    [ImposedBy] NVARCHAR(50) NOT NULL
)
