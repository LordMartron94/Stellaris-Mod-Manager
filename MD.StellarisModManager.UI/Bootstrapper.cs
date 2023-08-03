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
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using MD.Common;
using MD.StellarisModManager.UI.Library.Api;
using MD.StellarisModManager.UI.ViewModels;

namespace MD.StellarisModManager.UI;

public class Bootstrapper : BootstrapperBase
{
    private readonly SimpleContainer _container;

    private readonly ConfigurationEndpoint _configurationEndpoint;
    
    private readonly Dictionary<string, Action<string>> _argumentMethods;
    
    public Bootstrapper()
    {
        _container = new SimpleContainer();

        _configurationEndpoint = new ConfigurationEndpoint();

        _argumentMethods = new Dictionary<string, Action<string>>
        {
            {"stellarismoddeploymentpath",  _configurationEndpoint.SendStellarisModDeploymentOverride},
            {"stellarismodinstalllocation", _configurationEndpoint.AddStellarisModInstallLocation}
        };
        
        Initialize();   
    }

    protected override void Configure()
    {
        _container.Instance(_container)
            .PerRequest<ModEndpoint>()
            .PerRequest<ConfigurationEndpoint>();

        _container
            .Singleton<IWindowManager, WindowManager>()
            .Singleton<IEventAggregator, EventAggregator>();
        
        GetType().Assembly.GetTypes()
            .Where(type => type.IsClass)
            .Where(type => type.Name.EndsWith("ViewModel"))
            .ToList()
            .ForEach(viewModelType => _container.RegisterPerRequest(
                viewModelType, viewModelType.ToString(), viewModelType));
    }
    
    protected override void OnStartup(object sender, StartupEventArgs e)
    {
        HandleArgs(e.Args);
        
        DisplayRootViewForAsync<ShellViewModel>();
    }

    private void HandleArgs(IReadOnlyList<string> eArgs)
    {
        for (int i = 0; i < eArgs.Count; i++)
        {
            string arg = eArgs[i];
            
            
            // Validate argument as colon-separated string
            if (!arg.Contains(':'))
                throw new ArgumentException($"Argument at index {i} must be a colon-separated string.");

            KeyValuePair<string, string> keyValue = arg.ExtractKeyValueFromColonSeparatedString();
            
            // Basic validation
            if (string.IsNullOrWhiteSpace(keyValue.Key) || string.IsNullOrWhiteSpace(keyValue.Value))
                throw new ArgumentException($"Invalid key-value pair at index {i}. Both key and value must not be null or whitespace.");
            
            HandleKeyValuePair(keyValue);
        }
    }

    private void HandleKeyValuePair(KeyValuePair<string, string> keyValue)
    {
        if (!_argumentMethods.ContainsKey(keyValue.Key))
            return;
        
        _argumentMethods[keyValue.Key].Invoke(keyValue.Value);
    }

    protected override object GetInstance(Type service, string key)
    {
        return _container.GetInstance(service, key);
    }
    
    protected override IEnumerable<object> GetAllInstances(Type service)
    {
        return _container.GetAllInstances(service);
    }
    
    protected override void BuildUp(object instance)
    {
        _container.BuildUp(instance);
    }
}