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

using System.Windows;
using MD.StellarisModManager.UI.Library.Api;

namespace MD.StellarisModManager.UI.Library.ButtonHandling;

public class CheckNewMods : IButtonExecutor
{
    private readonly List<string> _buttonsToDisable;

    private Action<Visibility> _changeVisibilityMethod;
    private Action<string> _changeStatusTextMethod;

    private Action<double> _setProgressBarValueMethod;
    private Action<List<string>> _toggleModsMethod;

    private ModEndpoint _modEndpoint;
    
    public CheckNewMods(Action<Visibility> changeVisibilityMethod,
        Action<string> changeStatusTextMethod,
        Action<double> setProgressBarValueMethod,
        Action<List<string>> toggleModsMethod, 
        ModEndpoint modEndpoint)
    {
        _changeVisibilityMethod = changeVisibilityMethod;
        _changeStatusTextMethod = changeStatusTextMethod;
        _setProgressBarValueMethod = setProgressBarValueMethod;
        _toggleModsMethod = toggleModsMethod;
        _modEndpoint = modEndpoint;

        _buttonsToDisable = new List<string>
        {
            "CheckUpdates",
            "InstallNewMod",
            "Launch",
            "Sort"
        };
    }
    
    public async void Execute()
    {
        _changeVisibilityMethod.Invoke(Visibility.Visible);
        _changeStatusTextMethod.Invoke("Checking for new mods:");
    
        Progress<float> progressReporter = new Progress<float>(value =>
        {
            _setProgressBarValueMethod.Invoke(value * 100);
        });

        _toggleModsMethod.Invoke(_buttonsToDisable);
        await _modEndpoint.CheckForNewMods(progressReporter);
        _toggleModsMethod.Invoke(_buttonsToDisable);

        _setProgressBarValueMethod.Invoke(0);
        _changeStatusTextMethod.Invoke("");
        _changeVisibilityMethod.Invoke(Visibility.Collapsed);
    }
}