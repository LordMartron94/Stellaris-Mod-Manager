namespace MD.StellarisModManager.Core.Model;

public class ModListModel
{
    public string Name { get; set; }
    public string ID { get; set; }
    public Dictionary<ModDataModel, int> EnabledMods { get; set; }
}