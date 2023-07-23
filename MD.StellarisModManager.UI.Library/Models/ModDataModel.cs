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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using MD.StellarisModManager.Common;

namespace MD.StellarisModManager.UI.Library.Models;

public class ModDataModel : INotifyPropertyChanged
{
    #region Properties
    
    public int DatabaseId { get; set; }
    
    public ModDataRawModel Raw { get; set; }

    private int _displayPriority;
    public int OriginalDisplayPriority { get; private set; }
    
    public int DisplayPriority
    {
        get => _displayPriority;
        set
        {
            // ReSharper disable once InvertIf
            if (CanChangePriority(value))
            {
                OriginalDisplayPriority = _displayPriority;
                _displayPriority = value;
                OnPropertyChanged();
            }
        }
    }

    public FolderModel? DisplayFolder { get; set; }

    public ModCategory? ModCategory { get; set; }

    public RuleModel? AuthorRule { get; set; }
    public RuleModel? ModderRule { get; set; }

    public string? SmallDescription { get; set; }
    public string? ExtendedDescription { get; set; }

    private bool _enabled;

    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            OnPropertyChanged();
        }
    }
    
    #endregion
    
    #region Public Reference Props
    
    // TODO - Format names correctly
    public List<ModCategory> PossibleCategories => Enum.GetValues(typeof(ModCategory)).Cast<ModCategory>().ToList();
    
    #endregion
    
    #region Events

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    #endregion
    
    #region Checks

    public bool CanChangePriority(int value)
    {
        bool output = value >= 0;

        return output;
    }
    
    #endregion
}