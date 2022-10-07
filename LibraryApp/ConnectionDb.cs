using System;
using System.Data.SqlClient;

namespace LibraryApp;

internal class ConnectionDb : IDisposable
{
    public ConnectionDb()
    {
        SqlConnection = new SqlConnection(@"Server=(localdb)\MSSQLLocalDB;Database=Library");
        SqlConnection.Open();
    }

    public readonly SqlConnection SqlConnection;

    public void Dispose()
    {
        SqlConnection.Close();
    }
}