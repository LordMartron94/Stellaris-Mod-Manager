using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace MD.StellarisModManager.DataManager.Library.Internal.DataAccess;

internal class SqlDataAccess
{
    public string GetConnectionString(string name)
    {
        return
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MD.StellarisModManager.Data;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    }

    public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
    {
        string connectionString = GetConnectionString(connectionStringName);

        using IDbConnection connection = new SqlConnection(connectionString);
        
        List<T> rows = connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();
            
        return rows;
    }
    
    public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
    {
        string connectionString = GetConnectionString(connectionStringName);

        using IDbConnection connection = new SqlConnection(connectionString);
        
        connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }
}