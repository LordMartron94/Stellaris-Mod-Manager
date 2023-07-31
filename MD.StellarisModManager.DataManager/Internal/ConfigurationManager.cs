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

namespace MD.StellarisModManager.DataManager.Internal;

internal sealed class ConfigurationManager
{
    #region Properties
    
    private static ConfigurationManager? _instance = null;

    private string _stellarisModDeploymentPath;

    /// <summary>
    /// The directory where Stellaris has the descriptors for "deployed" mods.
    /// </summary>
    public string StellarisModDeploymentPath => _stellarisModDeploymentPath;
    
    private List<string> _stellarisModInstallDirectories;
    
    /// <summary>
    /// The directories where Stellaris or Steam originally has installed mods.
    /// </summary>
    public List<string> StellarisModInstallDirectories => _stellarisModInstallDirectories;
    
    private string _modInstallDirectory;
    
    /// <summary>
    /// The directory where the mod manager will put installed mods.
    /// </summary>
    public string ModInstallDirectory => _modInstallDirectory;
    
    #endregion
    
    #region Initialization
    
    private ConfigurationManager()
    {
        string docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string appPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        
        _stellarisModDeploymentPath = Path.Combine(docsPath, @"Paradox Interactive\Stellaris\mod");
        _stellarisModInstallDirectories = new List<string>();
        _modInstallDirectory = Path.Combine(appPath, @"Omega Code Solutions\StellarisModManager\Installed");
        
        Initialize();
    }

    public static ConfigurationManager GetInstance()
    {
        return _instance ??= new ConfigurationManager();
    }

    private void Initialize()
    {
        _stellarisModInstallDirectories.Add(@"D:\H. Happiness\Gaming\2. Games\Steam Library\steamapps\workshop\content\281990");
        
        VerifyPath(_modInstallDirectory, true);
        
        ClearDeployment();
    }

    private void ClearDeployment()
    {
        Utilities.DeleteFilesRecursively(_stellarisModDeploymentPath, "*.mod");
    }
    
    #endregion
    
    #region Paths
    
    /// <summary>
    /// Attempts to add a path where Stellaris or Steam downloads and installs mods.
    /// </summary>
    /// <param name="path">The path to set it to.</param>
    /// <param name="createDirectories">Whether or not to create (sub)directories if they don't exist.</param>
    public void AddStellarisInstallLocation(string path, bool createDirectories)
    {
        bool valid = VerifyPath(path, createDirectories);

        if (valid)
            _stellarisModInstallDirectories.Add(path);
    }

    /// <summary>
    /// Attempts to set the stellaris mod deployment path.
    /// </summary>
    /// <param name="path">The path to set it to.</param>
    /// <param name="createDirectories">Whether or not to create (sub)directories if they don't exist.</param>
    public void SetStellarisModDeploymentPath(string path, bool createDirectories)
    {
        bool valid = VerifyPath(path, createDirectories);

        if (valid)
            _stellarisModDeploymentPath = path;
    }
    
    /// <summary>
    /// Attempts to set the path where the manager will install mods.
    /// </summary>
    /// <param name="path">The path to set it to.</param>
    /// <param name="createDirectories">Whether or not to create (sub)directories if they don't exist.</param>
    public void SetModInstallPath(string path, bool createDirectories)
    {
        bool valid = VerifyPath(path, createDirectories);

        if (valid)
            _modInstallDirectory = path;
    }
    
    private bool VerifyPath(string stellarisModPath, bool createDirectories)
    {
        FileInfo fileInfo = new FileInfo(stellarisModPath);
        
        // Verify path is folder.
        if (!Directory.Exists(fileInfo.ToString()))
        {
            if (!createDirectories)
            {
                Console.WriteLine($"Is not a folder! {fileInfo}");
                return false;
            }
            
            Directory.CreateDirectory(fileInfo.ToString());
        }

        return true;
    }
    
    #endregion
}