CREATE PROCEDURE [dbo].[spModLookup]
	@Id int
AS
begin
	set nocount on;

	SELECT Id, DisplayPriority, DescriptionSmall, DescriptionExtended, RawData, Category, FolderID, AuthorRuleID, ModderRuleID
	FROM [dbo].[Mod]
	WHERE Id = @Id;
end
