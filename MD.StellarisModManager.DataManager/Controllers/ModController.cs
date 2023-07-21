﻿using MD.StellarisModManager.DataManager.Library.DataAccess;
using MD.StellarisModManager.DataManager.Library.Models;

namespace MD.StellarisModManager.DataManager.Controllers;

public class ModController
{
    private ModRepository _modRepository;

    public ModController()
    {
        _modRepository = new ModRepository();
    }
    
    #region Getters
    
    /// <summary>
    /// Gets a specific mod's information from the database.
    /// </summary>
    /// <param name="id">The ID of the mod to search for.</param>
    /// <returns>The found mod.</returns>
    public ModDataModel GetById(int id)
    {
        return _modRepository.GetModById(id).First();
    }

    /// <summary>
    /// Gets all mods in the database.
    /// </summary>
    /// <returns>A list of mods in the database.</returns>
    public List<ModDataModel> GetAll()
    {
        return _modRepository.GetAllMods();
    }
    
    #endregion
    
    #region Setters

    /// <summary>
    /// Adds a mod to the database.
    /// </summary>
    /// <param name="mod">The mod to add.</param>
    /// <returns>The ID of the mod.</returns>
    public int AddMod(ModDataModel mod)
    {
        return _modRepository.AddMod(mod);
    }
    
    #endregion
}