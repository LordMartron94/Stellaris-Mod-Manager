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
    
    private IConverterBi<DataManager.Models.Mod.FolderModel, FolderModel> _folderConverter;
    private IConverterBi<DataManager.Models.Mod.IncompatibilityModel, IncompatibilityModel> _incompatibilityConverter;
    private IConverterBi<DataManager.Models.Mod.RuleModel, RuleModel> _ruleConverter;
    private IConverterBi<DataManager.Models.Mod.ModDataRawModel, ModDataRawModel> _rawDataConverter;
    private IConverterBi<DataManager.Models.Mod.ModDataModel, ModDataModel> _modDataConverter;

    public ModEndpoint()
    {
        _modController = new ModController();

        _folderConverter = new FolderDataConverter();

        _incompatibilityConverter = new IncompatibilityConverter();
        _ruleConverter = new RuleConverter(_incompatibilityConverter);

        _rawDataConverter = new RawDataConverter();
        _modDataConverter = new ModDataConverter(_folderConverter, _ruleConverter, _rawDataConverter);
    }
    
    #region Getters
    
    public ModDataModel GetModInfo(int modId)
    {
        DataManager.Models.Mod.ModDataModel output = _modController.GetById(modId);

        return _modDataConverter.Convert(output);
    }
    
    public List<ModDataModel> GetModList()
    {
        List<DataManager.Models.Mod.ModDataModel> output = _modController.GetAll();

        List<ModDataModel> converted = output.Select(_modDataConverter.Convert).ToList();

        return converted;
    }
    
    #endregion

    #region Setters

    public void UpdateMod(ModDataModel modDataModel)
    {
        DataManager.Models.Mod.ModDataModel converted = _modDataConverter.ConvertBack(modDataModel);
        _modController.UpdateMod(converted);
    }

    #endregion
    
    #region Methods

    public async Task CheckForNewMods(IProgress<float>? progress)
    {
        await Task.Run(() => _modController.CheckNewMods(progress));
    }
    
    #endregion
}