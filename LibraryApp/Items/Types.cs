using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
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

    public static int Add(Type type)
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        const string query = "INSERT INTO Types ([name]) VALUES (@name)";
        using var cmd = new SqlCommand(query, con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("name", SqlDbType.NVarChar, 30) {Value = type.Name});
        return cmd.ExecuteNonQuery();
    }

    public static async Task<int> AddAsync(Type type)
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        const string query = "INSERT INTO Types ([name]) VALUES (@name)";
        await using var cmd = new SqlCommand(query, con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("name", SqlDbType.NVarChar, 30) {Value = type.Name});
        return await cmd.ExecuteNonQueryAsync();
    }
}