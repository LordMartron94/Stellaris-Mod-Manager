using System;
using Caliburn.Micro;

namespace MD.StellarisModManager.UI.ViewModels;

public class MainWindowViewModel : Screen
{
    public int ActiveMods { get; private set; } = 10;

    public void SetActiveMods(int amount)
    {
        ActiveMods = amount;
    }
    
    public void CheckUpdates()
    {
        Console.WriteLine("Checking for updates...");
    }

    public void CheckInstalled()
    {
        Console.WriteLine("Checking for installed mods...");
    }

    public void InstallNewMod()
    {
        Console.WriteLine("Installing new mod...");
    }

    public void Launch()
    {
        Console.WriteLine("Launching game...");
    }

    public void LoadOrder()
    {
        Console.WriteLine("Enabling Load order view...");
    }

    public void Saves()
    {
        Console.WriteLine("Enabling Saves view...");
    }

    public void Sort()
    {
        Console.WriteLine("Sorting Load Order...");
    }

    public void Export()
    {
        Console.WriteLine("Exporting Load Order...");
    }
    
    public void Import()
    {
        Console.WriteLine("Importing Load Order...");
    }
}