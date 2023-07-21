﻿CREATE PROCEDURE [dbo].[spModGetAll]
AS
begin
	set nocount on;

	SELECT Id, DisplayPriority, DescriptionSmall, DescriptionExtended, RawData, Category, FolderID, AuthorRuleID, ModderRuleID
	FROM [dbo].[Mod]
	ORDER BY [ID] ASC;
end