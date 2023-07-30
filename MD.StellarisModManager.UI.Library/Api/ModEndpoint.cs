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
using MD.StellarisModManager.DataManager.Controllers;
using MD.StellarisModManager.UI.Library.Api.Converters;
using MD.StellarisModManager.UI.Library.Models;

namespace MD.StellarisModManager.UI.Library.Api;

public class ModEndpoint
{
    private ModController _modController;
    
    private IConverter<DataManager.Models.FolderModel, FolderModel> _folderConverter;
    private IConverter<DataManager.Models.IncompatibilityModel, IncompatibilityModel> _incompatibilityConverter;
    private IConverter<DataManager.Models.RuleModel, RuleModel> _ruleConverter;
    private IConverter<DataManager.Models.ModDataRawModel, ModDataRawModel> _rawDataConverter;
    private IConverter<DataManager.Models.ModDataModel, ModDataModel> _modDataConverter;

    public ModEndpoint()
    {
        _modController = new ModController();

        _folderConverter = new FolderDataConverter();

        _incompatibilityConverter = new IncompatibilityConverter();
        _ruleConverter = new RuleConverter(_incompatibilityConverter);

        _rawDataConverter = new RawDataConverter();
        _modDataConverter = new ModDataConverter(_folderConverter, _ruleConverter, _rawDataConverter);
    }
    
    public ModDataModel GetModInfo(int modId)
    {
        DataManager.Models.ModDataModel output = _modController.GetById(modId);

        return _modDataConverter.Convert(output);
    }
    
    public List<ModDataModel> GetModList()
    {
        List<DataManager.Models.ModDataModel> output = _modController.GetAll();

        return output.Select(_modDataConverter.Convert).ToList();
    }
}