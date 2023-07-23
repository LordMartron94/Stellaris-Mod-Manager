using MD.StellarisModManager.UI.Library.Models;

namespace MD.StellarisModManager.UI.Library.Api.Helpers;

internal static class RawDataConversion
{
    internal static ModDataRawModel PublicToInternal(DataManager.Models.ModDataRawModel toConvert)
    {
        return new ModDataRawModel
        {
            ModName = toConvert.ModName,
            SupportedStellarisVersion = toConvert.SupportedStellarisVersion,
            ModVersion = toConvert.ModVersion,
            ModPath = toConvert.ModPath,
            RemoteFileID = toConvert.RemoteFileID,
            Picture = toConvert.Picture,
            Tags = toConvert.Tags,
            Dependencies = toConvert.Dependencies
        };
    }
}