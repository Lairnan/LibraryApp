using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Publishers
{
    public static IEnumerable<Publisher> GetPublishers()
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return GetPublisher(reader);
        }
    }
    public static async IAsyncEnumerable<Publisher> GetPublishersAsync()
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        await using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = await cmd.ExecuteReaderAsync();
        while (reader.Read())
        {
            yield return GetPublisher(reader);
        }
    }

    private static string GetString(string newQuery = "")
    {
        const string query = "SELECT * FROM [Publishers]";
        return query + newQuery;
    }

    private static Publisher GetPublisher(IDataRecord reader)
    {
        return new Publisher
        {
            Id = (int)reader["id"],
            Name = (string)reader["name"],
            Cipher = (string)reader["cipher"]
        };
    }
}