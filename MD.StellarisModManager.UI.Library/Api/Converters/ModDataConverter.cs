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

internal class ModDataConverter : IConverterBi<DataManager.Models.Mod.ModDataModel, ModDataModel>
{
    private IConverterBi<DataManager.Models.Mod.FolderModel, FolderModel> _folderConverter;
    private IConverterBi<DataManager.Models.Mod.RuleModel, RuleModel> _ruleConverter;
    private IConverterBi<DataManager.Models.Mod.ModDataRawModel, ModDataRawModel> _rawDataConverter;

    public ModDataConverter(IConverterBi<DataManager.Models.Mod.FolderModel, FolderModel> folderConverter,
        IConverterBi<DataManager.Models.Mod.RuleModel, RuleModel> ruleConverter,
        IConverterBi<DataManager.Models.Mod.ModDataRawModel, ModDataRawModel> rawDataConverter)
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

    public DataManager.Models.Mod.ModDataModel ConvertBack(ModDataModel toConvertBack)
    {
        DataManager.Models.Mod.FolderModel? displayFolder = null;
        DataManager.Models.Mod.RuleModel? authorRule = null;
        DataManager.Models.Mod.RuleModel? modderRule = null;

        if (toConvertBack.DisplayFolder != null)
            displayFolder = _folderConverter.ConvertBack(toConvertBack.DisplayFolder);
        
        if (toConvertBack.AuthorRule != null)
            authorRule = _ruleConverter.ConvertBack(toConvertBack.AuthorRule);
        
        if (toConvertBack.ModderRule != null)
            modderRule = _ruleConverter.ConvertBack(toConvertBack.ModderRule);
        
        DataManager.Models.Mod.ModDataModel mod = new DataManager.Models.Mod.ModDataModel
        {
            DatabaseId = toConvertBack.DatabaseId,
            
            Raw = _rawDataConverter.ConvertBack(toConvertBack.Raw),
            DisplayPriority = toConvertBack.DisplayPriority, 

            DisplayFolder = displayFolder,
            
            ModCategory = toConvertBack.ModCategory,
            AuthorRule = authorRule,
            ModderRule = modderRule,
            
            SmallDescription = toConvertBack.SmallDescription,
            ExtendedDescription = toConvertBack.ExtendedDescription,
            
            Enabled = toConvertBack.Enabled
        };
        
        return mod;
    }
}