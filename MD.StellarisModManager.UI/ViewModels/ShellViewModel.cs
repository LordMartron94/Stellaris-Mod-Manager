using Caliburn.Micro;

namespace MD.StellarisModManager.UI.ViewModels;

public class ShellViewModel : Conductor<object>
{
    private MainWindowViewModel _mainWindowViewModel;
    
    public ShellViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        ActivateItemAsync(mainWindowViewModel);
    }
}