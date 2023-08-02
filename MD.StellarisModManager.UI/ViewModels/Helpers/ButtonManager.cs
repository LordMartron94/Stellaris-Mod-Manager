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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows;
using MD.StellarisModManager.UI.Library.Api;
using MD.StellarisModManager.UI.Library.ButtonHandling;
using MD.StellarisModManager.UI.Models;

namespace MD.StellarisModManager.UI.ViewModels.Helpers;

public class ButtonManager : IButtonManager
{
    private readonly List<IButton> _buttons;
    
    private string? _operationExecuting;
    
    private ModEndpoint _modEndpoint;
    
    private double _progressBarValue;
    private string _progressBarStatusText;
    private Visibility _progressBarVisibility;
    
    public double ProgressBarValue
    {
        get => _progressBarValue;
        set
        {
            _progressBarValue = value;
            OnPropertyChanged();
        }
    }

    public string ProgressStatusText
    {
        get => _progressBarStatusText;
        set
        {
            _progressBarStatusText = value;
            OnPropertyChanged();
        }
    }

    public Visibility ProgressBarVisibility
    {
        get => _progressBarVisibility;
        set
        {
            _progressBarVisibility = value;
            OnPropertyChanged();
        }
    }

    [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
    public ButtonManager(ModEndpoint modEndpoint)
    {
        ProgressBarVisibility = Visibility.Collapsed;
        _progressBarStatusText = "";
        
        _buttons = new List<IButton>();

        _modEndpoint = modEndpoint;
        
        _buttons.Add(new Button("CheckUpdates", new CheckUpdates(), true));
        
        _buttons.Add(new Button("CheckInstalled", new CheckNewMods(
            changeVisibilityMethod: value => ProgressBarVisibility = value,
            changeStatusTextMethod:value => ProgressStatusText = value,
            setProgressBarValueMethod: value => ProgressBarValue = value,
            toggleModsMethod: value => ToggleButtonStates(value, "CheckInstalled"),
            modEndpoint: _modEndpoint), true));
        
        _buttons.Add(new Button("InstallNewMod", new InstallNewMod(), true));
        _buttons.Add(new Button("Launch", new Launch(), true));
        _buttons.Add(new Button("LoadOrder",new GoToLoadOrderView(), true));
        _buttons.Add(new Button("Saves", new GoToSavesView(), true));
        _buttons.Add(new Button("Sort", new SortLoadOrder(), true));
        _buttons.Add(new Button("Export", new ExportLoadOrder(), true));
        _buttons.Add(new Button("Import", new ImportLoadOrder(), true));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void ExecuteButton([CallerMemberName] string buttonName = "")
    {
        IButton? button = FindButton(buttonName);
        
        if (button == null)
            return;
        
        // Just in case, a safety check here too
        if (!button.Enabled)
            return;

        _operationExecuting = buttonName;
        button.Executor.Execute();
        _operationExecuting = null;
    }

    public bool GetButtonEnabled([CallerMemberName] string buttonName = "")
    {
        string convertedName = buttonName.Replace("Can", "");
        IButton? button = FindButton(convertedName);
        
        if (button == null)
            return false;

        return button.Enabled;
    }
    
    public void ToggleButtonState(string buttonName, [CallerMemberName] string operation = "")
    {
        IButton? button = FindButton(buttonName);
        
        if (button == null || (_operationExecuting != null && _operationExecuting != operation))
            return;
        
        button.SetButtonState(!button.Enabled);
        OnPropertyChanged($"Can{buttonName}");
    }

    public void ToggleButtonStates(List<string> buttonNames, [CallerMemberName] string operation = "")
    {
        if (_operationExecuting != null && _operationExecuting != operation)
            return;

        foreach (string buttonName in buttonNames)
            ToggleButtonState(buttonName, operation);
    }

    private IButton? FindButton(string buttonName)
    {
        IButton? button = _buttons.Find(b => b.Key == buttonName);

        if (button == null)
            Console.WriteLine($"Unknown button: {buttonName}");

        return button;
    }
}