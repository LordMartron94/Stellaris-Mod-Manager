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