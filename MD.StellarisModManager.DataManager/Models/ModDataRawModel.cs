namespace MD.StellarisModManager.DataManager.Models;

public class ModDataRawModel
{
    public string ModName { get; init; }
    public string SupportedStellarisVersion { get; init; }
    public string ModVersion { get; init; }

    public string ModPath { get; init; }
    // ReSharper disable once InconsistentNaming
    public string RemoteFileID { get; init; }
    public string Picture { get; init; }
    
    public string ModID => RemoteFileID;

    public List<string> Tags { get; init; }
    public List<string> Dependencies { get; init; }

    public ModDataRawModel(
        string modName,
        string supportedStellarisVersion,
        string modVersion,
        string modPath,
        // ReSharper disable once InconsistentNaming
        string remoteFileID,
        string picture,
        List<string> tags,
        List<string> dependencies)
    {
        ModName = modName;
        SupportedStellarisVersion = supportedStellarisVersion;
        ModVersion = modVersion;
        ModPath = modPath;
        RemoteFileID = remoteFileID;
        Picture = picture;
        Tags = tags;
        Dependencies = dependencies;
    }
}