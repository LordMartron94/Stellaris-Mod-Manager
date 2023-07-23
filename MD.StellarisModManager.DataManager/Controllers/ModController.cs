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

using MD.StellarisModManager.DataManager.Internal.Helpers;
using MD.StellarisModManager.DataManager.Library.DataAccess;
using MD.StellarisModManager.DataManager.Models;

namespace MD.StellarisModManager.DataManager.Controllers;

public class ModController
{
    private ModRepository _modRepository;

    public ModController()
    {
        _modRepository = new ModRepository();
    }
    
    #region Getters
    
    /// <summary>
    /// Gets a specific mod's information from the database.
    /// </summary>
    /// <param name="id">The Database ID of the mod to search for.</param>
    /// <returns>The found mod.</returns>
    public ModDataModel GetById(int id)
    {
        return DataConversion.InternalToPublic(_modRepository.GetModById(id));
    }

    /// <summary>
    /// Gets all mods in the database.
    /// </summary>
    /// <returns>A list of mods in the database.</returns>
    public List<ModDataModel> GetAll()
    {
        List<Library.Models.ModDataModel> raw = _modRepository.GetAllMods();

        List<ModDataModel> output = raw.Select(DataConversion.InternalToPublic).ToList();
        
        return output;
    }
    
    #endregion
    
    #region Setters

    /// <summary>
    /// Adds a mod to the database.
    /// </summary>
    /// <param name="mod">The mod to add.</param>
    /// <returns>The Database ID of the mod.</returns>
    public int AddMod(ModDataModel mod)
    {
        return _modRepository.AddMod(DataConversion.PublicToInternal(mod));
    }
    
    #endregion
}