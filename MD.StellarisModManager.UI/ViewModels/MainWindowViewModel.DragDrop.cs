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

using System.Windows;
using GongSolutions.Wpf.DragDrop;
using MD.StellarisModManager.UI.Library.Models;

namespace MD.StellarisModManager.UI.ViewModels;

public partial class MainWindowViewModel : IDropTarget
{
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
}