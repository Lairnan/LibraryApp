using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace LibraryApp;

internal class ConnectionDb : IDisposable
{
    public static async Task<ConnectionDb> ConnectionDbAsync()
    {
        var sqlConnection = new SqlConnection(@"Server=(localdb)\MSSQLLocalDB;Database=Library");
        await sqlConnection.OpenAsync();
        return new ConnectionDb(sqlConnection);
    }
    
    public ConnectionDb(SqlConnection sqlConnection)
    {
        this.SqlConnection = sqlConnection;
    }

    public readonly SqlConnection SqlConnection;

    public void Dispose()
    {
        SqlConnection.Close();
    }
}