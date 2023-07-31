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

using System.Text;
using System.Text.RegularExpressions;
using MD.StellarisModManager.DataManager.Models.Mod;

namespace MD.StellarisModManager.DataManager.Internal.Helpers;

public static class StellarisModFileInterpreter
{
    private static Dictionary<string, Action<ModDataRawModel, string>> _fieldSetters;
    private static Dictionary<string, Func<ModDataRawModel, string?>> _fieldGetters;
    
    static StellarisModFileInterpreter()
    {
        _fieldSetters = new Dictionary<string, Action<ModDataRawModel, string>>
        {
            ["name"] = (modDataRawModel, value) => modDataRawModel.ModName = value,
            ["picture"] = (modDataRawModel, value) => modDataRawModel.Picture = value,
            ["supported_version"] = (modDataRawModel, value) => modDataRawModel.SupportedStellarisVersion = value,
            ["version"] = (modDataRawModel, value) => modDataRawModel.ModVersion = value,
            ["path"] = (modDataRawModel, value) => modDataRawModel.ModPath = value,
            ["remote_file_id"] = (modDataRawModel, value) => modDataRawModel.RemoteFileID = value,
            ["archive"] = (modDataRawModel, value) => modDataRawModel.ArchivePath = value
        };
        
        _fieldGetters = new Dictionary<string, Func<ModDataRawModel, string?>>
        {
            ["name"] = (modDataRawModel) => modDataRawModel.ModName,
            ["picture"] = (modDataRawModel) => modDataRawModel.Picture,
            ["supported_version"] = (modDataRawModel) => modDataRawModel.SupportedStellarisVersion,
            ["version"] = (modDataRawModel) => modDataRawModel.ModVersion,
            ["path"] = (modDataRawModel) => modDataRawModel.ModPath,
            ["remote_file_id"] = (modDataRawModel) => modDataRawModel.RemoteFileID,
            ["archive"] = (modDataRawModel) => modDataRawModel.ArchivePath
        };
    }
    
    #region Set
    
    public static ModDataRawModel InterpretFile(string filePath)
    {
        ModDataRawModel modDataRawModel = new ModDataRawModel();
        string[] lines = File.ReadAllLines(filePath);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            Match match = Regex.Match(line, @"^(?<field>\w+)=""(?<value>.*)""$");

            if (match.Success)
                SetMatchedFields(modDataRawModel, match);
            else if (line.Contains("tags"))
                i = SetTags(modDataRawModel, lines, i);
            else if (line.Contains("dependencies"))
                i = SetDependencies(modDataRawModel, lines, i);
        }

        return modDataRawModel;
    }

    private static void SetMatchedFields(ModDataRawModel modDataRawModel, Match match)
    {
        string field = match.Groups["field"].Value;
        string value = match.Groups["value"].Value;

        if (_fieldSetters.TryGetValue(field, out Action<ModDataRawModel, string>? setField))
            setField(modDataRawModel, value);
        else
            Console.WriteLine($"Unknown field: {field}");
    }

    private static int SetTags(ModDataRawModel modDataRawModel, string[] lines, int currentIndex)
    {
        currentIndex++;

        while (!lines[currentIndex].Contains("}") && currentIndex < lines.Length)
        {
            string tag = lines[currentIndex].Trim();

            if (tag.StartsWith("\"") && tag.EndsWith("\""))
                tag = tag.Substring(1, tag.Length - 2);

            modDataRawModel.Tags.Add(tag);
            currentIndex++;
        }

        return currentIndex - 1;
    }

    private static int SetDependencies(ModDataRawModel modDataRawModel, string[] lines, int currentIndex)
    {
        currentIndex++;

        while (!lines[currentIndex].Contains("}") && currentIndex < lines.Length)
        {
            string dependency = lines[currentIndex].Trim();

            if (dependency.StartsWith("\"") && dependency.EndsWith("\""))
                dependency = dependency.Substring(1, dependency.Length - 2);

            modDataRawModel.Dependencies.Add(dependency);
            currentIndex++;
        }

        return currentIndex - 1;
    }
    
    #endregion
    
    #region Get
    
    public static string GetFileRepresentation(ModDataRawModel modDataRawModel)
    {
        StringBuilder builder = new StringBuilder();
        AppendFieldsValues(builder, modDataRawModel);
        AppendTags(builder, modDataRawModel);
        AppendDependencies(builder, modDataRawModel);
        return builder.ToString();
    }

    private static void AppendFieldsValues(StringBuilder builder, ModDataRawModel modDataRawModel)
    {
        foreach (KeyValuePair<string, Func<ModDataRawModel, string?>> fieldGetter in _fieldGetters)
        {
            string fieldName = fieldGetter.Key;
            string? fieldContent = fieldGetter.Value(modDataRawModel);
            if (fieldContent != null)
                builder.AppendLine($"{fieldName} = \"{fieldContent}\"");
        }
    }

    private static void AppendTags(StringBuilder builder, ModDataRawModel modDataRawModel)
    {
        if (modDataRawModel.Tags.Count > 0)
        {
            builder.AppendLine("tags = {");
            foreach (string tag in modDataRawModel.Tags)
                builder.AppendLine($"       \"{tag}\"");
            builder.AppendLine("}");
        }
    }

    private static void AppendDependencies(StringBuilder builder, ModDataRawModel modDataRawModel)
    {
        if (modDataRawModel.Dependencies.Count > 0)
        {
            builder.AppendLine("dependencies = {");
            foreach (string dependency in modDataRawModel.Dependencies)
                builder.AppendLine($"       \"{dependency}\"");
            builder.AppendLine("}");
        }
    }
    
    #endregion
}