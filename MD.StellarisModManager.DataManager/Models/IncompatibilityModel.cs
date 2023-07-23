using MD.StellarisModManager.Common;

namespace MD.StellarisModManager.DataManager.Models;

public class IncompatibilityModel
{
    public int AssociatedMod { get; set; }
    public int IncompatibleMod { get; set; }
    public IncompatibilityType IncompatibilityType { get; set; }
    public string Description { get; set; }
    public List<string> PossiblePatches { get; set; }
}