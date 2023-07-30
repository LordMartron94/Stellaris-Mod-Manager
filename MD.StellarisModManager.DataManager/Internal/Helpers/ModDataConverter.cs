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
using MD.StellarisModManager.Common;
using MD.StellarisModManager.DataManager.Models;
using Newtonsoft.Json;

namespace MD.StellarisModManager.DataManager.Internal.Helpers;

internal class ModDataConverter : IConverterBi<Library.Models.ModDataModel, ModDataModel>
{
    public ModDataModel Convert(Library.Models.ModDataModel toConvert)
    {
        // TODO - add conversion of Folders and Rules
        ModDataRawModel? deserializedRaw = JsonConvert.DeserializeObject<ModDataRawModel>(toConvert.RawData);

        if (deserializedRaw == null)
            throw new Exception("Could not deserialize raw data");

        Enum.TryParse(toConvert.Category, out ModCategory categoryEnumValue);

        bool enabled = toConvert.Enabled switch
        {
            0 => false,
            1 => true,
            _ => false
        };

        ModDataModel convertedModel = ModDataFactory.Create(toConvert.Id, deserializedRaw, toConvert.DisplayPriority, enabled, categoryEnumValue,
            toConvert.DescriptionSmall, toConvert.DescriptionExtended);
        
        return convertedModel;
    }

    public Library.Models.ModDataModel ConvertBack(ModDataModel toConvertBack)
    {
        // TODO - add conversion of Folders and Rules
        string serializedRaw = JsonConvert.SerializeObject(toConvertBack.Raw);
        
        string? modCategory = toConvertBack.ModCategory.ToString();

        int enabled = toConvertBack.Enabled? 1 : 0;

        Library.Models.ModDataModel convertedModel = Library.Models.Helpers.ModDataFactory.Create(toConvertBack.DatabaseId, serializedRaw,
            toConvertBack.DisplayPriority, enabled, modCategory, toConvertBack.SmallDescription, toConvertBack.ExtendedDescription,
            toConvertBack.DisplayFolder?.FolderID, toConvertBack.AuthorRule?.RuleID, toConvertBack.ModderRule?.RuleID);
        
        return convertedModel;
    }
}