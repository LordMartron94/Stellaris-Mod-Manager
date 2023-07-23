namespace MD.StellarisModManager.DataManager.Models;

public class ModDataRawModel
{
    public string ModName { get; set; }
    public string SupportedStellarisVersion { get; set; }
    public string ModVersion { get; set; }

    public string ModPath { get; set; }
    // ReSharper disable once InconsistentNaming
    public string RemoteFileID { get; set; }
    public string Picture { get; set; }
    
    public string ModID => RemoteFileID;

    public List<string> Tags { get; set; }
    public List<string> Dependencies { get; set; }
}