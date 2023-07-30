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
using MD.StellarisModManager.UI.Library.Models;

namespace MD.StellarisModManager.UI.ViewModels.Helpers.Strategies;

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

        _displayChangedStrategies = new Dictionary<DisplayChangeCase, IDisplayChangedStrategy>()
        {
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
        _modSorter.SortByDisplayCount(_installedMods);

        _currentlyUpdatingDisplayCount = false;
    }

    private void HandleDisplayPriorityChange(ModDataModel modToChange)
    {
        int highest = GetHighestDisplayPriority(modToChange);
        
        switch (modToChange.DisplayPriority)
        {
            case var priority when priority > highest:
                // Even though it seems redundant, we have to store the original display priority before we truncate, otherwise the algorithm will update mods that are between the pre-truncated value and the truncated value.
                int originalDisplayPriority = modToChange.OriginalDisplayPriority;
                modToChange.DisplayPriority = highest;
                _displayChangedStrategies[DisplayChangeCase.AboveMax].Handle(ref _installedMods, modToChange, originalDisplayPriority, modToChange.DisplayPriority);
                break;
            case var priority when priority == highest:
                _displayChangedStrategies[DisplayChangeCase.IsMax].Handle(ref _installedMods, modToChange, modToChange.OriginalDisplayPriority, modToChange.DisplayPriority);
                break;
            case var priority when modToChange.OriginalDisplayPriority > priority:
                _displayChangedStrategies[DisplayChangeCase.GoesDown].Handle(ref _installedMods, modToChange, modToChange.OriginalDisplayPriority, modToChange.DisplayPriority);
                break;
            case var priority when modToChange.OriginalDisplayPriority < priority:
                _displayChangedStrategies[DisplayChangeCase.GoesUp].Handle(ref _installedMods, modToChange, modToChange.OriginalDisplayPriority, modToChange.DisplayPriority);
                break;
            default:
                Console.WriteLine("Unknown variation of display priority change!");
                break;
        }
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