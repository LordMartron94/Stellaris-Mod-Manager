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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MD.StellarisModManager.UI.Library.Extensions;
using MD.StellarisModManager.UI.Library.Models;
using MD.StellarisModManager.UI.Library.ModSorting;
using MD.StellarisModManager.UI.Library.PropertyChangeHandling;
using MD.StellarisModManager.UI.Library.PropertyChangeHandling.DisplayCountHandling;

namespace MD.StellarisModManager.UI.ViewModels.Helpers;

public class ModCollectionHandler : IModCollectionHandler
{
    private Dictionary<string, IPropertyChangeStrategy> _propertyChangeStrategies;
    
    private IModSorter _modSorter;

    private BindingList<ModDataModel> _installedMods;

    public BindingList<ModDataModel> InstalledMods
    {
        get => _installedMods;
        set
        {
            _installedMods = value;
            OnPropertyChanged();
        }
    }

    public ModCollectionHandler()
    {
        _installedMods = new BindingList<ModDataModel>();
        
        _modSorter = new ModSorter();
        
        _propertyChangeStrategies = new Dictionary<string, IPropertyChangeStrategy>
        {
            { nameof(ModDataModel.Enabled), new EnabledChangeStrategy() },
            { nameof(ModDataModel.DisplayPriority), new DisplayCountChangeStrategy(_modSorter, ref _installedMods) }
        };
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void OnPropertyChange(object? sender, PropertyChangedEventArgs e)
    {
        if (_propertyChangeStrategies.TryGetValue(e.PropertyName!, out IPropertyChangeStrategy? strategy))
            strategy.HandlePropertyChange(sender);
        else
            Console.WriteLine("Unknown property change: " + e.PropertyName);
    }

    public void AddMods(IEnumerable<ModDataModel> mods, bool autoSort = true)
    {
        List<ModDataModel> converted = mods.ToList();

        InstalledMods.AddRange(converted);

        foreach (ModDataModel installedMod in converted)
            installedMod.PropertyChanged += OnPropertyChange;
        
        if (autoSort)
            _modSorter.SortByDisplayCount(InstalledMods);
    }
}