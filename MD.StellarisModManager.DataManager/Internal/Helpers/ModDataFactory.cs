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

using MD.StellarisModManager.Common;
using MD.StellarisModManager.DataManager.Models;
using MD.StellarisModManager.DataManager.Models.Mod;

namespace MD.StellarisModManager.DataManager.Internal.Helpers;

public static class ModDataFactory
{
    public static ModDataModel Create(
        int dbId,
        ModDataRawModel rawData,
        int displayPriority,
        bool enabled,
        ModCategory? category = null,
        string? smallDescription = null,
        string? extendedDescription = null,
        FolderModel? displayFolder = null,
        RuleModel? authorRuleId = null,
        RuleModel? modderRuleId = null)
    {
        ModDataModel model = new ModDataModel
        {
            DatabaseId = dbId,
            Raw = rawData,
            DisplayPriority = displayPriority,
            DisplayFolder = displayFolder,
            ModCategory = category,
            AuthorRule = authorRuleId,
            ModderRule = modderRuleId,
            SmallDescription = smallDescription,
            ExtendedDescription = extendedDescription,
            Enabled = enabled
        };

        return model;
    }
}