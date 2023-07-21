namespace MD.StellarisModManager.UI.Library.Models;

public class ModListModel
{
    public string Name { get; set; }
    public string ID { get; set; }
    public Dictionary<ModDataModel, int> EnabledMods { get; set; }
}