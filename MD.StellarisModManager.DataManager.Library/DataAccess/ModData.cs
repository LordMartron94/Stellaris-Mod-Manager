using MD.StellarisModManager.DataManager.Library.Internal.DataAccess;
using MD.StellarisModManager.DataManager.Library.Models;

namespace MD.StellarisModManager.DataManager.Library.DataAccess;

public class ModData
{
    public List<ModDataModel> GetModById(int Id)
    {
        SqlDataAccess sql = new SqlDataAccess();

        dynamic p = new { Id = Id };

        dynamic? output = sql.LoadData<ModDataModel, dynamic>("spModLookup", p, "DefaultConnection");

        return output;
    }
}