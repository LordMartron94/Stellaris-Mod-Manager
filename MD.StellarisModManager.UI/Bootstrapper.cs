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
using MD.StellarisModManager.UI.Library.Api;
using MD.StellarisModManager.UI.ViewModels;

namespace MD.StellarisModManager.UI;

public class Bootstrapper : BootstrapperBase
{
    private readonly SimpleContainer _container;
    
    public Bootstrapper()
    {
        _container = new SimpleContainer();
        
        Initialize();   
    }

    protected override void Configure()
    {
        _container.Instance(_container)
            .PerRequest<ModEndpoint>();

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
        DisplayRootViewForAsync<ShellViewModel>();
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