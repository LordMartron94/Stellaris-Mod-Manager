using System.Diagnostics.CodeAnalysis;
using MD.StellarisModManager.DataManager.Library.Internal;
using MD.StellarisModManager.DataManager.Library.Internal.DataAccess;
using MD.StellarisModManager.DataManager.Library.Models;

namespace MD.StellarisModManager.DataManager.Library.DataAccess;

public class ModRepository
{
    private SqlDataAccess _sql;
    
    public ModRepository()
    {
        _sql = new SqlDataAccess();
    }

    #region Getters
    
    public List<ModDataModel> GetAllMods()
    {
        dynamic? output = _sql.LoadData<ModDataModel>("spModGetAll", Connection.Default);

        return output;
    }
    
    public List<ModDataModel> GetModById(int id)
    {
        dynamic p = new { Id = id };

        dynamic? output = _sql.LoadData<ModDataModel, dynamic>("spModLookup", p, Connection.Default);

        return output;
    }
    
    #endregion
    
    #region Setters
    
    [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
    public int AddMod(ModDataModel mod)
    {
        dynamic p = new
        {
            DisplayPriority = mod.DisplayPriority,
            DescriptionSmall = mod.DescriptionSmall,
            DescriptionExtended = mod.DescriptionExtended,
            RawData = mod.RawData,
            Category = mod.Category,
            FolderID = mod.FolderID,
            AuthorRuleID = mod.AuthorRuleID,
            ModderRuleID = mod.ModderRuleID
        };
        
        dynamic? output = _sql.SaveData<dynamic>("spAddMod", p, Connection.Default);

        return output;
    }
    
    #endregion
}