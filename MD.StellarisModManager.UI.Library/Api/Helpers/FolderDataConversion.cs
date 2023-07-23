using MD.StellarisModManager.UI.Library.Models;

namespace MD.StellarisModManager.UI.Library.Api.Helpers;

internal static class FolderDataConversion
{
    internal static FolderModel PublicToInternal(DataManager.Models.FolderModel toConvert)
    {
        return new FolderModel
        {
            FolderName = toConvert.FolderName,
            FolderID = toConvert.FolderID,
            DisplayPriority = toConvert.DisplayPriority
        };
    }
}