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

using MD.Common;
using MD.StellarisModManager.DataManager.Internal.Helpers;
using MD.StellarisModManager.DataManager.Models.Mod;

namespace MD.StellarisModManager.DataManager.Internal;

public class ModHandler
{
    private ConfigurationManager _configurationManager;

    public ModHandler()
    {
        _configurationManager = ConfigurationManager.GetInstance();
    }

    public void CheckForNewMods(IProgress<float>? progress = null)
    {
        List<string> stellarisInstallLocations = _configurationManager.StellarisModInstallDirectories;
        List<DirectoryInfo> stellarisInstallDirectories = stellarisInstallLocations.Select(location => new DirectoryInfo(location)).ToList();
    
        DirectoryInfo modInstallLocation = new DirectoryInfo(_configurationManager.ModInstallDirectory);

        int totalMods = stellarisInstallDirectories.SelectMany(dir => dir.GetDirectories()).Count();
        int processedMods = 0;

        foreach (DirectoryInfo[] subdirs in stellarisInstallDirectories.Select(stellarisInstallDirectory => stellarisInstallDirectory.GetDirectories()))
        {
            MoveMods(modInstallLocation, subdirs, () =>
            {
                processedMods++;
                float percentComplete = (float)processedMods / totalMods;
                progress?.Report(percentComplete); // Report progress
            });
        }
    }

    private void MoveMods(DirectoryInfo modInstallLocation, DirectoryInfo[] modFolders, Action onModProcessed)
    {
        foreach (DirectoryInfo dir in modFolders)
        {
            FileInfo modDescriptor = dir.GetFiles().Where(f => f.Name.Contains("descriptor") && f.Extension == ".mod").ToList()[0];
            ModDataRawModel interpretedDescriptor = StellarisModFileInterpreter.InterpretFile(modDescriptor.ToString());

            string modInstallPath = Path.Combine(modInstallLocation.ToString(), $@"{interpretedDescriptor.ModName}");
            
            Utilities.MoveFilesRecursively(dir.ToString(), modInstallPath);

            onModProcessed(); // Notify of processed mod
        }
    }
}