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

namespace MD.StellarisModManager.DataManager.Library.Models.Helpers;

public static class ModDataFactory
{
    public static ModDataModel CreateNew(
        string rawData,
        int displayPriority,
        int enabled,
        string? category = null,
        string? smallDescription = null,
        string? extendedDescription = null,
        int? displayFolderId = null,
        int? authorRuleId = null,
        int? modderRuleId = null)
    {
        ModDataModel model = new ModDataModel
        {
            RawData = rawData,
            Enabled = enabled,
            DisplayPriority = displayPriority,
            FolderID = displayFolderId,
            Category = category,
            AuthorRuleID = authorRuleId,
            ModderRuleID = modderRuleId,
            DescriptionSmall = smallDescription,
            DescriptionExtended = extendedDescription
        };

        return model;
    }  
    
    public static ModDataModel CreateExisting(
        int dbId,
        string rawData,
        int displayPriority,
        int enabled,
        string? category = null,
        string? smallDescription = null,
        string? extendedDescription = null,
        int? displayFolderId = null,
        int? authorRuleId = null,
        int? modderRuleId = null)
    {
        ModDataModel model = new ModDataModel
        {
            Id = dbId,
            RawData = rawData,
            Enabled = enabled,
            DisplayPriority = displayPriority,
            FolderID = displayFolderId,
            Category = category,
            AuthorRuleID = authorRuleId,
            ModderRuleID = modderRuleId,
            DescriptionSmall = smallDescription,
            DescriptionExtended = extendedDescription
        };

        return model;
    }  
}