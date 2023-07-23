using MD.StellarisModManager.UI.Library.Models;

namespace MD.StellarisModManager.UI.Library.Api.Helpers;

internal static class ModDataConversion
{
    internal static ModDataModel PublicToInternal(DataManager.Models.ModDataModel toConvert)
    {
        FolderModel? displayFolder = null;
        RuleModel? authorRule = null;
        RuleModel? modderRule = null;

        if (toConvert.DisplayFolder != null)
            displayFolder = FolderDataConversion.PublicToInternal(toConvert.DisplayFolder);
        
        if (toConvert.AuthorRule != null)
            authorRule = RuleDataConversion.PublicToInternal(toConvert.AuthorRule);
        
        if (toConvert.ModderRule != null)
            modderRule = RuleDataConversion.PublicToInternal(toConvert.ModderRule);
        
        ModDataModel mod = new ModDataModel
        {
            DatabaseId = toConvert.DatabaseId,
            
            Raw = RawDataConversion.PublicToInternal(toConvert.Raw),
            DisplayPriority = toConvert.DisplayPriority, 

            DisplayFolder = displayFolder,
            
            ModCategory = toConvert.ModCategory,
            AuthorRule = authorRule,
            ModderRule = modderRule,
            
            SmallDescription = toConvert.SmallDescription,
            ExtendedDescription = toConvert.ExtendedDescription,
            
            Enabled = toConvert.Enabled
        };
        
        return mod;
    }
}