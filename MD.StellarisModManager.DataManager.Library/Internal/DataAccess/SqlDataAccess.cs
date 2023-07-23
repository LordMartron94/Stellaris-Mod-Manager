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

using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace MD.StellarisModManager.DataManager.Library.Internal.DataAccess;

internal class SqlDataAccess
{
    public string GetConnectionString(Connection type)
    {
        return type switch
        {
            Connection.Default =>
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MD.StellarisModManager.Data;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public List<T> LoadData<T, U>(string storedProcedure, U parameters, Connection connectionType)
    {
        string connectionString = GetConnectionString(connectionType);

        using IDbConnection connection = new SqlConnection(connectionString);
        
        List<T> rows = connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();
            
        return rows;
    }
    
    public List<T> LoadData<T>(string storedProcedure, Connection connectionType)
    {
        string connectionString = GetConnectionString(connectionType);

        using IDbConnection connection = new SqlConnection(connectionString);
        
        List<T> rows = connection.Query<T>(storedProcedure, commandType: CommandType.StoredProcedure).ToList();
            
        return rows;
    }
    
    public int SaveData<T>(string storedProcedure, T parameters, Connection connectionType)
    {
        string connectionString = GetConnectionString(connectionType);

        using IDbConnection connection = new SqlConnection(connectionString);

        return connection.QuerySingle<int>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }
}