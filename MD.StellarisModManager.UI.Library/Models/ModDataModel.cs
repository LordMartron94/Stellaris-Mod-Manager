using System.ComponentModel;
using System.Runtime.CompilerServices;
using MD.StellarisModManager.Common;

namespace MD.StellarisModManager.UI.Library.Models;

public class ModDataModel : INotifyPropertyChanged
{
    #region Properties
    
    public int DatabaseId { get; set; }
    
    public ModDataRawModel Raw { get; set; }

    private int _displayPriority;
    public int OriginalDisplayPriority { get; private set; }
    
    public int DisplayPriority
    {
        get => _displayPriority;
        set
        {
            // ReSharper disable once InvertIf
            if (CanChangePriority(value))
            {
                OriginalDisplayPriority = _displayPriority;
                _displayPriority = value;
                OnPropertyChanged();
            }
        }
    }

    public FolderModel? DisplayFolder { get; set; }

    public ModCategory? ModCategory { get; set; }

    public RuleModel? AuthorRule { get; set; }
    public RuleModel? ModderRule { get; set; }

    public string? SmallDescription { get; set; }
    public string? ExtendedDescription { get; set; }

    private bool _enabled;

    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            OnPropertyChanged();
        }
    }
    
    #endregion
    
    #region Public Reference Props
    
    // TODO - Format names correctly
    public List<ModCategory> PossibleCategories => Enum.GetValues(typeof(ModCategory)).Cast<ModCategory>().ToList();
    
    #endregion
    
    #region Events

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    #endregion
    
    #region Checks

    public bool CanChangePriority(int value)
    {
        bool output = value >= 0;

        return output;
    }
    
    #endregion
}