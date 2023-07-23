using MD.StellarisModManager.Common;
using MD.StellarisModManager.DataManager.Models;
using Newtonsoft.Json;

namespace MD.StellarisModManager.DataManager.Internal.Helpers;

internal static class DataConversion
{
    internal static ModDataModel InternalToPublic(Library.Models.ModDataModel toConvert)
    {
        // TODO - add conversion of Folders and Rules
        ModDataRawModel? deserializedRaw = JsonConvert.DeserializeObject<ModDataRawModel>(toConvert.RawData);

        if (deserializedRaw == null)
            throw new Exception("Could not deserialize raw data");

        Enum.TryParse(toConvert.Category, out ModCategory categoryEnumValue);

        ModDataModel convertedModel = new ModDataModel
        {
            DatabaseId = toConvert.Id,
            Raw = deserializedRaw,
            DisplayPriority = toConvert.DisplayPriority,
            DisplayFolder = null,
            ModCategory = categoryEnumValue,
            AuthorRule = null,
            ModderRule = null,
            SmallDescription = toConvert.DescriptionSmall,
            ExtendedDescription = toConvert.DescriptionExtended
        };
        
        return convertedModel;
    }

    internal static Library.Models.ModDataModel PublicToInternal(ModDataModel toConvert)
    {
        // TODO - add conversion of Folders and Rules
        string serializedRaw = JsonConvert.SerializeObject(toConvert.Raw);
        
        string? modCategory = toConvert.ModCategory.ToString();

        int enabled = toConvert.Enabled? 1 : 0;
        
        Library.Models.ModDataModel convertedModel = new Library.Models.ModDataModel
        {
            Id = toConvert.DatabaseId,
            DisplayPriority = toConvert.DisplayPriority,
            DescriptionSmall = toConvert.SmallDescription,
            DescriptionExtended = toConvert.ExtendedDescription,
            RawData = serializedRaw,
            Category = modCategory,
            FolderID = toConvert.DisplayFolder?.FolderID,
            AuthorRuleID = toConvert.AuthorRule?.RuleID,
            ModderRuleID = toConvert.ModderRule?.RuleID,
            Enabled = enabled
        };
        
        return convertedModel;
    }
}