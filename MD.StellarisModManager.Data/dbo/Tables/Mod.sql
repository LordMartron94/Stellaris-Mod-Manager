CREATE TABLE [dbo].[Mod]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DisplayPriority] INT NOT NULL, 
    [DescriptionSmall] TEXT NULL, 
    [DescriptionExtended] TEXT NULL, 
    [RawData] NVARCHAR(MAX) NOT NULL, 
    [Category] NVARCHAR(50) NULL, 
    [FolderID] INT NULL, 
    [AuthorRuleID] INT NULL, 
    [ModderRuleID] INT NULL,
    [Enabled] INT NOT NULL
)
