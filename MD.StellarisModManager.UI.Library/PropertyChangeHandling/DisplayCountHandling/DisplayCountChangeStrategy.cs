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

using System.ComponentModel;
using MD.StellarisModManager.UI.Library.Models;
using MD.StellarisModManager.UI.Library.ModSorting;
using MD.StellarisModManager.UI.Library.PropertyChangeHandling.DisplayCountHandling.Strategies;

namespace MD.StellarisModManager.UI.Library.PropertyChangeHandling.DisplayCountHandling;

public class DisplayCountChangeStrategy : IPropertyChangeStrategy
{
    private bool _currentlyUpdatingDisplayCount;

    private readonly IModSorter _modSorter;

    private BindingList<ModDataModel> _installedMods;

    private enum DisplayChangeCase
    {
        AboveMax,
        IsMax,
        GoesUp,
        GoesDown,
        IsMin,
        BelowMin,
    }

    private Dictionary<DisplayChangeCase, IDisplayChangedStrategy> _displayChangedStrategies;

    public DisplayCountChangeStrategy(IModSorter modSorter, ref BindingList<ModDataModel> installedMods)
    {
        _modSorter = modSorter;
        _installedMods = installedMods;

        _displayChangedStrategies = new Dictionary<DisplayChangeCase, IDisplayChangedStrategy>
        {
            {DisplayChangeCase.BelowMin, new DisplayBelowMinStrategy()},
            {DisplayChangeCase.IsMin, new DisplayMinStrategy()},
            {DisplayChangeCase.AboveMax, new DisplayAboveMaxStrategy()},
            {DisplayChangeCase.IsMax, new DisplayMaxStrategy()},
            {DisplayChangeCase.GoesUp, new DisplayGoesUpStrategy()},
            {DisplayChangeCase.GoesDown, new DisplayGoesDownStrategy()}
        };
    }
    
    public void HandlePropertyChange(object? sender)
    {
        if (_currentlyUpdatingDisplayCount)
            return;
        if (sender == null)
            throw new ArgumentNullException(nameof(sender));

        _currentlyUpdatingDisplayCount = true;
        
        ModDataModel modToChange = (ModDataModel)sender;
        
        HandleDisplayPriorityChange(modToChange);
        // _modSorter.SortByDisplayCount(_installedMods);
        
        _currentlyUpdatingDisplayCount = false;
    }

    private void HandleDisplayPriorityChange(ModDataModel modToChange)
    {
        int highest = GetHighestDisplayPriority(modToChange);
        int lowest = GetLowestDisplayPriority(modToChange);

        DisplayChangeCase changeCase = GetChangeCase(modToChange, highest, lowest);

        // We must store this now, otherwise if it is below or above min/max then it will cause weird bugs.
        // Where each mod will change between the after truncated and before truncated value instead of the after truncated and new value.
        int originalDisplayCount = modToChange.OriginalDisplayPriority;

        modToChange.DisplayPriority = changeCase switch
        {
            DisplayChangeCase.AboveMax => highest,
            DisplayChangeCase.BelowMin => lowest,
            _ => modToChange.DisplayPriority
        };

        _displayChangedStrategies[changeCase].Handle(ref _installedMods, modToChange, originalDisplayCount, modToChange.DisplayPriority);
    }

    private DisplayChangeCase GetChangeCase(ModDataModel modToChange, int highest, int lowest)
    {
        int priority = modToChange.DisplayPriority;
        
        if (priority > highest) return DisplayChangeCase.AboveMax;
        if (priority == highest) return DisplayChangeCase.IsMax;
        if (priority < lowest) return DisplayChangeCase.BelowMin;
        if (priority == lowest) return DisplayChangeCase.IsMin;
        
        return modToChange.OriginalDisplayPriority > priority ? DisplayChangeCase.GoesDown : DisplayChangeCase.GoesUp;
    }

    private int GetLowestDisplayPriority(ModDataModel modToFilterOut)
    {
        int lowest = int.MaxValue;
        
        foreach (ModDataModel modToCheck in _installedMods)
        {
            if (modToCheck == modToFilterOut)
                continue;
            
            if (modToCheck.DisplayPriority < lowest)
                lowest = modToCheck.DisplayPriority;
        }
        
        return lowest;
    }

    private int GetHighestDisplayPriority(ModDataModel modToFilterOut)
    {
        int highest = 0;
        
        foreach (ModDataModel modToCheck in _installedMods)
        {
            if (modToCheck == modToFilterOut)
                continue;
            
            if (modToCheck.DisplayPriority > highest)
                highest = modToCheck.DisplayPriority;
        }
        
        return highest;
    }
}