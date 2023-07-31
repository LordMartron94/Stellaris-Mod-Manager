CREATE PROCEDURE spUpdateMod
    @Id INT,
    @DisplayPriority INT,
    @DescriptionSmall TEXT,
    @DescriptionExtended TEXT,
    @RawData NVARCHAR(MAX),
    @Category NVARCHAR(50),
    @FolderID INT,
    @AuthorRuleID INT,
    @ModderRuleID INT,
    @Enabled INT
AS
BEGIN
    UPDATE [dbo].[Mod]
    SET 
        DisplayPriority = @DisplayPriority,
        DescriptionSmall = @DescriptionSmall,
        DescriptionExtended = @DescriptionExtended,
        RawData = @RawData,
        Category = @Category,
        FolderID = @FolderID,
        AuthorRuleID = @AuthorRuleID,
        ModderRuleID = @ModderRuleID,
        [Enabled] = @Enabled
    WHERE Id = @Id;

    SELECT Id FROM [dbo].[Mod] WHERE Id = @Id;
END;