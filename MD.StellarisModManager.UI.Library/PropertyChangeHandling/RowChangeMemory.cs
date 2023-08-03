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

using System.Diagnostics;
using MD.StellarisModManager.UI.Library.Api;
using MD.StellarisModManager.UI.Library.Models;

namespace MD.StellarisModManager.UI.Library.PropertyChangeHandling;

public class RowChangeMemory
{
    private List<ModDataModel> _changedMods;

    private static RowChangeMemory? _instance = null;

    private ModEndpoint _endpoint;
    
    private RowChangeMemory()
    {
        _endpoint = new ModEndpoint();
        
        _changedMods = new List<ModDataModel>();
    }

    public static RowChangeMemory GetInstance()
    {
        return _instance ??= new RowChangeMemory();
    }

    public void ModChanged(ModDataModel changed)
    {
        if (_changedMods.Contains(changed))
            return;
        
        _changedMods.Add(changed);
    }

    public void SaveChanges(bool debug = false)
    {
        Stopwatch sw = new Stopwatch();
        
        sw.Start();

        foreach (ModDataModel modDataModel in _changedMods)
        {
            if (debug)
                Console.WriteLine($"Saving mod: {modDataModel.Raw.ModID} with ID {modDataModel.DatabaseId} and display-count {modDataModel.DisplayPriority}");
            
            _endpoint.UpdateMod(modDataModel);
        }
        
        sw.Stop();
        
        if (debug)
            Console.WriteLine($"Time spent to save changes: {sw.ElapsedMilliseconds}ms");
    }
}