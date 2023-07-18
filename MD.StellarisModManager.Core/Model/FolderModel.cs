namespace MD.StellarisModManager.Core.Model;

public class FolderModel
{
    public string FolderName { get; set; }
    public string FolderID { get; set; }
    public int DisplayPriority { get; set; }
    public List<ModDataModel> ModsContained { get; set; }
}