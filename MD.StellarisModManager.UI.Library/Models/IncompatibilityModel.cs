namespace MD.StellarisModManager.UI.Library.Models;

public class IncompatibilityModel
{
    public ModDataModel AssociatedMod { get; set; }
    public ModDataModel IncompatibleMod { get; set; }
    public IncompatibilityType IncompatibilityType { get; set; }
    public string Description { get; set; }
    public List<string> PossiblePatches { get; set; }
}