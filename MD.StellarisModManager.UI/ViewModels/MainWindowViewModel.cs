#region Copyright

//  Stellaris Mod Manager used to manage a library of installed mods for the game of Stellaris
// Copyright (C) 2023  Matthew David van der Hoorn
//
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, at version 3 of the license.
//
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//     You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//
// CONTACT:
// Email: md.vanderhoorn@gmail.com
//     Business Email: admin@studyinstitute.net
// Discord: mr.hoornasp.learningexpert
// Phone: +31 6 18206979

#endregion

using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Caliburn.Micro;
using MD.StellarisModManager.UI.Library.Api;
using MD.StellarisModManager.UI.Library.Models;
using MD.StellarisModManager.UI.Library.PropertyChangeHandling;
using MD.StellarisModManager.UI.ViewModels.Helpers;

namespace MD.StellarisModManager.UI.ViewModels;

public partial class MainWindowViewModel : Screen
{
    private readonly ModEndpoint _modEndpoint;
    private readonly IModCollectionHandler _modCollectionHandler;

    public int ActiveMods { get; private set; }

    private readonly IButtonManager _buttonManager;
    
    private readonly RowChangeMemory _changeMemory;
    private ConfigurationEndpoint _configurationEndpoint;

    public double ProgressBarValue
    {
        get => _buttonManager.ProgressBarValue;
        set
        {
            _buttonManager.ProgressBarValue = value;
            NotifyOfPropertyChange(() => ProgressBarValue);
        }
    }
    
    public string ProgressStatusText
    {
        get => _buttonManager.ProgressStatusText;
        set
        {
            _buttonManager.ProgressStatusText = value;
            NotifyOfPropertyChange(() => ProgressStatusText);
        }
    }

    public Visibility ProgressBarVisibility
    {
        get => _buttonManager.ProgressBarVisibility;
        set
        {
            _buttonManager.ProgressBarVisibility = value;
            NotifyOfPropertyChange(() => ProgressBarVisibility);
        }
    }
    
    public ICollectionView ModView { get; private set; }

    public BindingList<ModDataModel> InstalledMods => _modCollectionHandler.InstalledMods;

    public MainWindowViewModel(ModEndpoint modEndpoint, ConfigurationEndpoint configurationEndpoint)
    {
        _modEndpoint = modEndpoint;
        _configurationEndpoint = configurationEndpoint;
        
        _changeMemory = RowChangeMemory.GetInstance();
        
        _buttonManager = new ButtonManager(_modEndpoint);
        _buttonManager.PropertyChanged += OnPropertyChanged;
        
        _modCollectionHandler = new ModCollectionHandler();

        ModView = CollectionViewSource.GetDefaultView(InstalledMods);
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        NotifyOfPropertyChange(e.PropertyName);
    }

    protected override void OnViewLoaded(object view)
    {
        base.OnViewLoaded(view);
        Initialize();
    }

    protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
    {
        _changeMemory.SaveChanges(_configurationEndpoint.GetDebug());
        return base.OnDeactivateAsync(close, cancellationToken);
    }

    private void Initialize()
    {
        _modCollectionHandler.AddMods(_modEndpoint.GetModList(), false);
        
        // if (ModView.SortDescriptions.Count > 0)
        //     ModView.SortDescriptions.Clear();
        //
        // ModView.SortDescriptions.Add(new SortDescription("DisplayPriority", ListSortDirection.Ascending));
        
        Console.WriteLine($"Can group: {ModView.CanSort}");
    }
    
    #region ButtonHandling
    
    public void CheckUpdates() => _buttonManager.ExecuteButton();
    public void CheckInstalled() => _buttonManager.ExecuteButton();
    public void InstallNewMod() => _buttonManager.ExecuteButton();
    public void Launch() => _buttonManager.ExecuteButton();
    public void LoadOrder() => _buttonManager.ExecuteButton();
    public void Saves() => _buttonManager.ExecuteButton();
    public void Sort() => _buttonManager.ExecuteButton();
    public void Export() => _buttonManager.ExecuteButton();
    public void Import() => _buttonManager.ExecuteButton();
    
    #endregion
    
    #region ButtonLocks

    public bool CanCheckUpdates => _buttonManager.GetButtonEnabled();
    public bool CanCheckInstalled => _buttonManager.GetButtonEnabled();
    public bool CanInstallNewMod => _buttonManager.GetButtonEnabled();
    public bool CanLaunch => _buttonManager.GetButtonEnabled();
    public bool CanLoadOrder => _buttonManager.GetButtonEnabled();
    public bool CanSaves => _buttonManager.GetButtonEnabled();
    public bool CanSort => _buttonManager.GetButtonEnabled();
    public bool CanExport => _buttonManager.GetButtonEnabled();
    public bool CanImport => _buttonManager.GetButtonEnabled();
    
    #endregion
}