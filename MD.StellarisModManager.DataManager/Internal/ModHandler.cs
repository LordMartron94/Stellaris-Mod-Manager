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
using MD.StellarisModManager.DataManager.Library.DataAccess;
using MD.StellarisModManager.DataManager.Models.Mod;
using Newtonsoft.Json;

namespace MD.StellarisModManager.DataManager.Internal;

public class ModHandler
{
    private ConfigurationManager _configurationManager;

    private ModRepository _modRepository;
    private ModDataConverter _modDataConverter;

    public ModHandler(ModRepository repository)
    {
        _configurationManager = ConfigurationManager.GetInstance();
        
        _modDataConverter = new ModDataConverter();
        
        _modRepository = repository;
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
                progress?.Report(percentComplete);
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

            AddModToDatabase(interpretedDescriptor);
            
            onModProcessed();
        }
    }

    // The only reason I comment this in production code and do not remove it entirely is because chances are I need this again when I add new features
    // And I am too lazy to create a full replacement in my test project for this. :)
    // public void AddModsToDatabaseTest()
    // {
    //     DirectoryInfo modInstallLocation = new DirectoryInfo(_configurationManager.ModInstallDirectory);
    //
    //     foreach (DirectoryInfo modDir in modInstallLocation.GetDirectories())
    //     {
    //         try
    //         {
    //             FileInfo modDescriptor = modDir.GetFiles().Where(f => f.Name.Contains("descriptor") && f.Extension == ".mod").ToList()[0];
    //             ModDataRawModel interpretedDescriptor = StellarisModFileInterpreter.InterpretFile(modDescriptor.ToString());
    //
    //             Console.WriteLine($"Adding {interpretedDescriptor.ModName} to the database!");
    //
    //             bool success = AddModToDatabase(interpretedDescriptor);
    //             
    //             if(success)
    //                 Console.WriteLine($"Successfully added {interpretedDescriptor.ModName} to the database!");
    //         }
    //         catch (Exception e)
    //         {
    //             Console.WriteLine($"Error adding {modDir.Name} - {e}");
    //         }
    //     }
    // }

    private bool AddModToDatabase(ModDataRawModel rawData)
    {
        if (!CanAdd(rawData))
        {
            Console.WriteLine($"Mod {rawData.ModName} with ID {rawData.ModID} already in database! Cannot add duplicates!");
            return false;
        }

        // TODO - Add Category, Descriptions, Folder, Rules

        string serializedRaw = JsonConvert.SerializeObject(rawData);
        int displayPriority = GetMaxDisplayPriority() + 1;

        Library.Models.ModDataModel model = Library.Models.Helpers.ModDataFactory.CreateNew(
            rawData: serializedRaw,
            displayPriority: displayPriority,
            enabled: 0,
            null,
            null,
            null,
            null,
            null,
            null);
        
        _modRepository.AddMod(model);

        return true;
    }

    private int GetMaxDisplayPriority()
    {
        List<Library.Models.ModDataModel> currentInstalledMods = _modRepository.GetAllMods().OrderByDescending(m => m.DisplayPriority).ToList();

        return currentInstalledMods.Count <= 0 ? 0 : currentInstalledMods[0].DisplayPriority;
    }

    private bool CanAdd(ModDataRawModel model)
    {
        bool output = false;

        List<string> modsInDb = _modRepository.GetAllMods().Select(m => JsonConvert.DeserializeObject<ModDataRawModel>(m.RawData).ModID).ToList();

        if (!modsInDb.Contains(model.ModID))
            output = true;

        return output;
    }
}