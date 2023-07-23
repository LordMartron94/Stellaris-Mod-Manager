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
using System.Windows;
using Caliburn.Micro;
using GongSolutions.Wpf.DragDrop;
using MD.StellarisModManager.UI.Library.Api;
using MD.StellarisModManager.UI.Library.Models;

// TODO - Clean this class up (SOLID refactoring)

namespace MD.StellarisModManager.UI.ViewModels;

public class MainWindowViewModel : Screen, IDropTarget
{
    private ModEndpoint _modEndpoint;

    // private List<ModDataModel> _installedModsBacking;
    private BindingList<ModDataModel> _installedMods;

    private List<int> _itemIdsLastChangedByDragDrop;

    private List<int> _manualCountChange;

    public int ActiveMods { get; private set; }

    public MainWindowViewModel(ModEndpoint modEndpoint)
    {
        _modEndpoint = modEndpoint;
        
        _installedMods = new BindingList<ModDataModel>();
        _itemIdsLastChangedByDragDrop = new List<int>();
        _manualCountChange = new List<int>{0};
    }

    protected override void OnViewLoaded(object view)
    {
        base.OnViewLoaded(view);

        // _installedModsBacking = _modEndpoint.GetModList();

        List<ModDataModel> installedMods = _modEndpoint.GetModList();

        foreach (ModDataModel mod in installedMods)
            InstalledMods.Add(mod);
        
        SortInstalledMods();

        foreach (ModDataModel installedMod in InstalledMods)
            installedMod.PropertyChanged += OnPropertyChange;
    }

    private void SortInstalledMods()
    {
        InstalledMods = new BindingList<ModDataModel>(_installedMods.OrderBy(mod => mod.DisplayPriority).ToList());
    }

    #region Properties

    public BindingList<ModDataModel> InstalledMods
    {
        get => _installedMods;
        set
        {
            _installedMods = value;
            NotifyOfPropertyChange(() => InstalledMods);
        }
    }
    
    #endregion
    
    #region Events

    private void OnPropertyChange(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ModDataModel.Enabled))
            OnEnabledChange();
        else if (e.PropertyName == nameof(ModDataModel.DisplayPriority))
            OnDisplayCountChange(sender);
    }
    
    private void OnEnabledChange()
    {
        // TODO - Add mod to the load order list
        Console.WriteLine("Mod toggled");
    }

    private void OnDisplayCountChange(object? sender)
    {
        if (sender == null)
            throw new ArgumentNullException(nameof(sender));

        ModDataModel modToChange = (ModDataModel)sender;

        // Uses weird cache system to ensure that only the root call actually executes the algorithm to prevent weird bugs.
        // Only the first call has the same item in the list for both the last and first item in the list.
        // I saw no other way to make a cache system that works because it's a class, and a method variable has no use for cache.
        // A simple boolean value has no use. Wait, as I write this, I notice that a simple boolean value would work lmao. 
        // It's late, 2.25 am, but for the purpose of having fun, I will leave this weird caching system in place.
        if (_itemIdsLastChangedByDragDrop.Contains(modToChange.DatabaseId) || _manualCountChange.Last() != _manualCountChange.First())
            return;
        
        _manualCountChange.Add(_manualCountChange.Max() + 1);
    
        int highest = GetHighestDisplayPriority(modToChange);
        
        if (modToChange.DisplayPriority > highest)
        {
            int originalDisplayPriority = modToChange.OriginalDisplayPriority;
            int truncatedDisplayPriority = highest;
            modToChange.DisplayPriority = truncatedDisplayPriority;
    
            DecreaseDisplayPrioritiesInBetweenNewAndOldDisplayPriority(modToChange, originalDisplayPriority, highest);
        }
        else if (modToChange.DisplayPriority == highest)
        {
            int originalDisplayPriority = modToChange.OriginalDisplayPriority;
            DecreaseDisplayPrioritiesInBetweenNewAndOldDisplayPriority(modToChange, originalDisplayPriority, highest);
        }
        else if (modToChange.OriginalDisplayPriority > modToChange.DisplayPriority)
            IncreaseDisplayPrioritiesInBetweenNewAndOldDisplayPriority(modToChange, modToChange.OriginalDisplayPriority, modToChange.DisplayPriority);
        else if (modToChange.OriginalDisplayPriority < modToChange.DisplayPriority)
            DecreaseDisplayPrioritiesInBetweenNewAndOldDisplayPriority(modToChange, modToChange.OriginalDisplayPriority, modToChange.DisplayPriority);
        else
            Console.WriteLine("Unknown variation of display priority change!");
        
        _manualCountChange = new List<int>{0};
        
        SortInstalledMods();
    }

    private int GetHighestDisplayPriority(ModDataModel modToFilterOut)
    {
        int highest = 0;
        
        foreach (ModDataModel modToCheck in InstalledMods)
        {
            if (modToCheck == modToFilterOut)
                continue;
            
            if (modToCheck.DisplayPriority > highest)
                highest = modToCheck.DisplayPriority;
        }
        
        return highest;
    }

    private void DecreaseDisplayPrioritiesInBetweenNewAndOldDisplayPriority(ModDataModel originalMod, int originalDisplayPriority, int newDisplayPriority, bool dragDrop = false)
    {
        foreach (ModDataModel modToDetriment in InstalledMods)
        {
            if (modToDetriment == originalMod)
                continue;
                
            if (modToDetriment.DisplayPriority < originalDisplayPriority || modToDetriment.DisplayPriority > newDisplayPriority)
                continue;

            if (dragDrop)
                _itemIdsLastChangedByDragDrop.Add(modToDetriment.DatabaseId);
            
            modToDetriment.DisplayPriority -= 1;
        }
    }

    private void IncreaseDisplayPrioritiesInBetweenNewAndOldDisplayPriority(ModDataModel originalMod, int originalDisplayPriority, int newDisplayPriority, bool dragDrop = false)
    {
        foreach (ModDataModel modToDetriment in InstalledMods)
        {
            if (modToDetriment == originalMod)
                continue;
                
            if (modToDetriment.DisplayPriority < newDisplayPriority || modToDetriment.DisplayPriority > originalDisplayPriority)
                continue;

            if (dragDrop)
                _itemIdsLastChangedByDragDrop.Add(modToDetriment.DatabaseId);
            
            modToDetriment.DisplayPriority += 1;
        }
    }

    #endregion

    #region Buttons
    
    public void CheckUpdates()
    {
        Console.WriteLine("Checking for updates...");
    }

    public void CheckInstalled()
    {
        Console.WriteLine("Checking for installed mods...");
    }

    public void InstallNewMod()
    {
        Console.WriteLine("Installing new mod...");
    }

    public void Launch()
    {
        Console.WriteLine("Launching game...");
    }

    public void LoadOrder()
    {
        Console.WriteLine("Enabling Load order view...");
    }

    public void Saves()
    {
        Console.WriteLine("Enabling Saves view...");
    }

    public void Sort()
    {
        Console.WriteLine("Sorting Load Order...");
    }

    public void Export()
    {
        Console.WriteLine("Exporting Load Order...");
    }
    
    public void Import()
    {
        Console.WriteLine("Importing Load Order...");
    }
    
    #endregion

    public void DragOver(IDropInfo dropInfo)
    {
        ModDataModel? sourceItem = (ModDataModel?)dropInfo.Data;
        ModDataModel? targetItem = (ModDataModel?)dropInfo.TargetItem;

        if (sourceItem != null && targetItem != null)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Move;
        }
    }
    
    public void Drop(IDropInfo dropInfo)
    {
        ModDataModel? sourceItem = (ModDataModel?)dropInfo.Data;
        ModDataModel? targetItem = (ModDataModel?)dropInfo.TargetItem;

        if (targetItem == null || sourceItem == null || targetItem == sourceItem)
            return;

        // Need to use cache system, otherwise the OnPropertyChange Event will interfere with the algorithm here -- in essence things would be fired twice.
        // Took me at least two hours to find the cause of this _bug.
        _itemIdsLastChangedByDragDrop.Clear();
        _itemIdsLastChangedByDragDrop.Add(sourceItem.DatabaseId);
        
        int toDropPriority = targetItem.DisplayPriority;

        // Add new item
        int indexOfDrop = InstalledMods.IndexOf(targetItem);
        
        // Check desired move behavior
        if (targetItem.DisplayPriority > sourceItem.DisplayPriority)
        {
            int highestDisplayPriority = GetHighestDisplayPriority(sourceItem);
            
            // IF MAXIMUM PRIORITY
            if (targetItem.DisplayPriority == highestDisplayPriority)
                DecreaseDisplayPrioritiesInBetweenNewAndOldDisplayPriority(sourceItem, sourceItem.DisplayPriority, highestDisplayPriority, dragDrop:true);
            else
                DecreaseDisplayPrioritiesInBetweenNewAndOldDisplayPriority(sourceItem, sourceItem.DisplayPriority, toDropPriority, dragDrop:true);
        }
        else if (targetItem.DisplayPriority < sourceItem.DisplayPriority)
           IncreaseDisplayPrioritiesInBetweenNewAndOldDisplayPriority(sourceItem, sourceItem.DisplayPriority, toDropPriority, dragDrop:true);
        
        sourceItem.DisplayPriority = toDropPriority;

        // Remove original item
        InstalledMods.Remove(sourceItem);
        InstalledMods.Insert(indexOfDrop, sourceItem);
        
        // Reset cache
        _itemIdsLastChangedByDragDrop.Clear();
    }
}