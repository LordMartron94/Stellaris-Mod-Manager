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
using System.Windows;
using Caliburn.Micro;
using GongSolutions.Wpf.DragDrop;
using MD.StellarisModManager.UI.Library.Api;
using MD.StellarisModManager.UI.Library.Models;
using MD.StellarisModManager.UI.ViewModels.Helpers;

namespace MD.StellarisModManager.UI.ViewModels;

public class MainWindowViewModel : Screen, IDropTarget
{
    private readonly ModEndpoint _modEndpoint;

    private readonly IModCollectionHandler _modCollectionHandler;

    public int ActiveMods { get; private set; }
    
    public BindingList<ModDataModel> InstalledMods => _modCollectionHandler.InstalledMods;

    public MainWindowViewModel(ModEndpoint modEndpoint)
    {
        _modEndpoint = modEndpoint;

        _modCollectionHandler = new ModCollectionHandler();
    }

    protected override void OnViewLoaded(object view)
    {
        base.OnViewLoaded(view);
        Initialize();
    }

    private void Initialize()
    {
        _modCollectionHandler.AddMods(_modEndpoint.GetModList());
    }

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

    #region Drag and Drop
    
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
        
        bool sourceExists = sourceItem != null;
        bool targetExists = targetItem != null;
        bool sourceAndTargetAreDifferent = sourceItem != targetItem;

        bool validDrop = (sourceExists && targetExists) && sourceAndTargetAreDifferent;

        if (!validDrop)
            return;

        sourceItem!.DisplayPriority = targetItem!.DisplayPriority;
    }
    
    #endregion
}