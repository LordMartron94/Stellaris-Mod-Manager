using MD.StellarisModManager.DataManager.Controllers;
using MD.StellarisModManager.UI.Library.Api.Helpers;
using MD.StellarisModManager.UI.Library.Models;

namespace MD.StellarisModManager.UI.Library.Api;

public class ModEndpoint
{
    private ModController _modController;

    public ModEndpoint()
    {
        _modController = new ModController();
    }
    
    public ModDataModel GetModInfo(int modId)
    {
        DataManager.Models.ModDataModel output = _modController.GetById(modId);

        return ModDataConversion.PublicToInternal(output);
    }
    
    public List<ModDataModel> GetModList()
    {
        List<DataManager.Models.ModDataModel> output = _modController.GetAll();

        return output.Select(ModDataConversion.PublicToInternal).ToList();
    }
}