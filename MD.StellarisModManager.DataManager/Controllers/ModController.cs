using MD.StellarisModManager.DataManager.Library.DataAccess;
using MD.StellarisModManager.DataManager.Library.Models;

namespace MD.StellarisModManager.DataManager.Controllers;

public class ModController
{
    public ModDataModel GetById(int id)
    {
        ModData modData = new ModData();
        
        return modData.GetModById(id).First();
    }
}