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

using MD.StellarisModManager.DataManager.Controllers;

namespace MD.StellarisModManager.UI.Library.Api;

public class ConfigurationEndpoint
{
    private ConfigurationController _configurationController;

    public ConfigurationEndpoint()
    {
        _configurationController = new ConfigurationController();
    }
    
    /// <summary>
    /// Note: If the path is invalid, this will simply be ignored.
    /// </summary>
    public void SendStellarisModDeploymentOverride(string path)
    {
        _configurationController.OverrideStellarisModDeploymentPath(path);
    }

    /// <summary>
    /// Note: If the path is invalid, this will simply be ignored.
    /// </summary>
    public void AddStellarisModInstallLocation(string path)
    {
        _configurationController.AddStellarisModInstallLocation(path);
    }
}