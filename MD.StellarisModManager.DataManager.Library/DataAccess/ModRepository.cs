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


using System.Diagnostics.CodeAnalysis;
using MD.StellarisModManager.DataManager.Library.Internal;
using MD.StellarisModManager.DataManager.Library.Internal.DataAccess;
using MD.StellarisModManager.DataManager.Library.Models;

namespace MD.StellarisModManager.DataManager.Library.DataAccess;

public class ModRepository
{
    private SqlDataAccess _sql;
    
    public ModRepository()
    {
        _sql = new SqlDataAccess();
    }

    #region Getters
    
    public List<ModDataModel> GetAllMods()
    {
        List<ModDataModel> output = _sql.LoadData<ModDataModel>("spModGetAll", Connection.Default);

        return output;
    }
    
    public ModDataModel GetModById(int id)
    {
        dynamic p = new { Id = id };

        List<ModDataModel> output = _sql.LoadData<ModDataModel, dynamic>("spModLookup", p, Connection.Default);

        return output[0];
    }
    
    #endregion
    
    #region Setters
    
    [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
    public int AddMod(ModDataModel mod)
    {
        dynamic p = new
        {
            DisplayPriority = mod.DisplayPriority,
            DescriptionSmall = mod.DescriptionSmall,
            DescriptionExtended = mod.DescriptionExtended,
            RawData = mod.RawData,
            Category = mod.Category,
            FolderID = mod.FolderID,
            AuthorRuleID = mod.AuthorRuleID,
            ModderRuleID = mod.ModderRuleID,
            Enabled = mod.Enabled
        };
        
        dynamic? output = _sql.SaveData<dynamic>("spAddMod", p, Connection.Default);
    
        return output;
    }
    
    #endregion
}