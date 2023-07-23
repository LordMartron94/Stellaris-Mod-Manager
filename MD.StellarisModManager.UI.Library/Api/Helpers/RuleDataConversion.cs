using MD.StellarisModManager.UI.Library.Models;

namespace MD.StellarisModManager.UI.Library.Api.Helpers;

internal static class RuleDataConversion
{
    internal static RuleModel PublicToInternal(DataManager.Models.RuleModel toConvert)
    {
        List<IncompatibilityModel> incompatibilities = 
            toConvert.Incompatibilities.Select(IncompatibilityDataConversion.PublicToInternal).ToList();

        return new RuleModel
        {
            RuleID = toConvert.RuleID,
            LoadAfter = toConvert.LoadAfter,
            LoadBefore = toConvert.LoadBefore,
            Incompatibilities = incompatibilities,
            AssociatedMod = toConvert.AssociatedMod,
            ImposedBy = toConvert.ImposedBy
        };
    }
}