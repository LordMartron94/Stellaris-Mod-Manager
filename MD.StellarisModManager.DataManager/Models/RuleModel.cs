using MD.StellarisModManager.Common;

namespace MD.StellarisModManager.DataManager.Models;

public class RuleModel
{
    public int RuleID { get; set; }
    public List<int> LoadAfter { get; set; }
    public List<int> LoadBefore { get; set; }
    public List<IncompatibilityModel> Incompatibilities { get; set; }
    public int AssociatedMod { get; set; }
    public ImposedBy ImposedBy { get; set; }
}