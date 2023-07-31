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
using MD.StellarisModManager.UI.Library.Models;

namespace MD.StellarisModManager.UI.Library.Api.Converters;

internal class ModDataConverter : IConverter<DataManager.Models.Mod.ModDataModel, ModDataModel>
{
    private IConverter<DataManager.Models.Mod.FolderModel, FolderModel> _folderConverter;
    private IConverter<DataManager.Models.Mod.RuleModel, RuleModel> _ruleConverter;
    private IConverter<DataManager.Models.Mod.ModDataRawModel, ModDataRawModel> _rawDataConverter;

    public ModDataConverter(IConverter<DataManager.Models.Mod.FolderModel, FolderModel> folderConverter,
        IConverter<DataManager.Models.Mod.RuleModel, RuleModel> ruleConverter,
        IConverter<DataManager.Models.Mod.ModDataRawModel, ModDataRawModel> rawDataConverter)
    {
        _folderConverter = folderConverter;
        _ruleConverter = ruleConverter;
        _rawDataConverter = rawDataConverter;
    }
    
    public ModDataModel Convert(DataManager.Models.Mod.ModDataModel toConvert)
    {
        FolderModel? displayFolder = null;
        RuleModel? authorRule = null;
        RuleModel? modderRule = null;

        if (toConvert.DisplayFolder != null)
            displayFolder = _folderConverter.Convert(toConvert.DisplayFolder);
        
        if (toConvert.AuthorRule != null)
            authorRule = _ruleConverter.Convert(toConvert.AuthorRule);
        
        if (toConvert.ModderRule != null)
            modderRule = _ruleConverter.Convert(toConvert.ModderRule);
        
        ModDataModel mod = new ModDataModel
        {
            DatabaseId = toConvert.DatabaseId,
            
            Raw = _rawDataConverter.Convert(toConvert.Raw),
            DisplayPriority = toConvert.DisplayPriority, 

            DisplayFolder = displayFolder,
            
            ModCategory = toConvert.ModCategory,
            AuthorRule = authorRule,
            ModderRule = modderRule,
            
            SmallDescription = toConvert.SmallDescription,
            ExtendedDescription = toConvert.ExtendedDescription,
            
            Enabled = toConvert.Enabled
        };
        
        return mod;
    }
}