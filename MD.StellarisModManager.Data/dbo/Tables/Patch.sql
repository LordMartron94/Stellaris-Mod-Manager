CREATE TABLE [dbo].[Patch]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [IncompatibilityId] INT NOT NULL, 
    [Url] NVARCHAR(400) NOT NULL
)
