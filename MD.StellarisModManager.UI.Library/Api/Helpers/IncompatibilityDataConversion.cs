using MD.StellarisModManager.UI.Library.Models;

namespace MD.StellarisModManager.UI.Library.Api.Helpers;

internal static class IncompatibilityDataConversion
{
    internal static IncompatibilityModel PublicToInternal(DataManager.Models.IncompatibilityModel toConvert)
    {
        return new IncompatibilityModel
        {
            AssociatedMod = toConvert.AssociatedMod,
            IncompatibleMod = toConvert.IncompatibleMod,
            IncompatibilityType = toConvert.IncompatibilityType,
            Description = toConvert.Description,
            PossiblePatches = toConvert.PossiblePatches
        };
    }
}