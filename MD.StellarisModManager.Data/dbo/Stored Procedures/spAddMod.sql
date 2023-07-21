CREATE PROCEDURE [dbo].[spAddMod]
	@DisplayPriority int,
	@DescriptionSmall text = null,
	@DescriptionExtended text = null,
	@RawData nvarchar(max),
	@Category nvarchar(50) = null,
	@FolderID int = null,
	@AuthorRuleID int = null,
	@ModderRuleID int = null
AS
begin
	-- Perform the insertion
	INSERT INTO [dbo].[Mod](DisplayPriority, DescriptionSmall, DescriptionExtended, RawData, Category, FolderID, AuthorRuleID, ModderRuleID)
	VALUES (@DisplayPriority, @DescriptionSmall, @DescriptionExtended, @RawData, @Category, @FolderID, @AuthorRuleID, @ModderRuleID)

	-- Return the Item ID of the inserted mod
	SELECT SCOPE_IDENTITY();
end
