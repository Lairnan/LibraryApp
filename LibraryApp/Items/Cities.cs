using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
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

    public static int Add(City city)
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        const string query = "INSERT INTO Cities ([name]) VALUES (@name)";
        using var cmd = new SqlCommand(query, con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("name", SqlDbType.NVarChar, 30) {Value = city.Name});
        return cmd.ExecuteNonQuery();
    }

    public static async Task<int> AddAsync(City city)
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        const string query = "INSERT INTO Cities ([name]) VALUES (@name)";
        await using var cmd = new SqlCommand(query, con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("name", SqlDbType.NVarChar, 30) {Value = city.Name});
        return await cmd.ExecuteNonQueryAsync();
    }
}