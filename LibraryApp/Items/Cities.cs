using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Cities
{
    public static IEnumerable<City> Get()
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return GetCity(reader);
        }
    }
    public static async IAsyncEnumerable<City> GetAsync()
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        await using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = await cmd.ExecuteReaderAsync();
        while (reader.Read())
        {
            yield return GetCity(reader);
        }
    }

    private static string GetString(string newQuery = "")
    {
        const string query = "SELECT * FROM [Cities]";
        return query + newQuery;
    }

    private static City GetCity(IDataRecord reader)
    {
        return new City
        {
            Id = (int)reader["id"],
            Name = (string)reader["name"]
        };
    }
}