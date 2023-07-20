using MD.StellarisModManager.DataManager.Library;

namespace MD.StellarisModManager.DataManager.Models;

public class RuleModel
{
    public string RuleID { get; set; }
    public List<ModDataModel> LoadAfter { get; set; }
    public List<ModDataModel> LoadBefore { get; set; }
    public List<IncompatibilityModel> Incompatibilities { get; set; }
    public ModDataModel AssociatedMod { get; set; }
    public ImposedBy ImposedBy { get; set; }
}