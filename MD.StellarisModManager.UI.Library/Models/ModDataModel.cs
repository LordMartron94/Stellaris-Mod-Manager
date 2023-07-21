namespace MD.StellarisModManager.UI.Library.Models;

public class ModDataModel
{
    public ModDataRawModel Raw { get; set; }

    public int DisplayPriority { get; set; }
    public FolderModel DisplayFolder { get; set; }

    public ModCategory ModCategory { get; set; }

    public RuleModel AuthorRule { get; set; }
    public RuleModel ModderRule { get; set; }

    public string SmallDescription { get; set; }
    public string ExtendedDescription { get; set; }
}