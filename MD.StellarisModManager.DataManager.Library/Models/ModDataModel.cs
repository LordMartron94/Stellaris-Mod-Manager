namespace MD.StellarisModManager.DataManager.Library.Models;

public class ModDataModel
{
    public int Id { get; set; }
    public int DisplayPriority { get; set; }
    public string DescriptionSmall { get; set; }
    public string DescriptionExtended { get; set; }
    public string RawData { get; set; }
    public string Category { get; set; }
    public int FolderID { get; set; }
    public int AuthorRuleID { get; set; }
    public int ModderRuleID { get; set; }
}