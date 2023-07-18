CREATE TABLE [dbo].[Incompatibility]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AssociatedRuleId] INT NOT NULL,
    [AssociatedModId] INT NOT NULL, 
    [IncompatibleModId] INT NOT NULL, 
    [IncompatibilityType] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL
)
