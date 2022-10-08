using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Types
{
    public static IEnumerable<Type> Get()
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return GetType(reader);
        }
    }
    public static async IAsyncEnumerable<Type> GetAsync()
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        await using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = await cmd.ExecuteReaderAsync();
        while (reader.Read())
        {
            yield return GetType(reader);
        }
    }

    private static string GetString(string newQuery = "")
    {
        const string query = "SELECT * FROM [Types]";
        return query + newQuery;
    }

    private static Type GetType(IDataRecord reader)
    {
        return new Type
        {
            Id = (int) reader["id"],
            Name = (string) reader["name"]
        };
    }
}