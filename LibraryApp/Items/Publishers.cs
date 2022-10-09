using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Publishers
{
    public static IEnumerable<Publisher> Get()
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return GetPublisher(reader);
        }
    }
    public static async IAsyncEnumerable<Publisher> GetAsync()
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
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

    public static int Add(Publisher publisher)
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        const string query = "INSERT INTO Publishers ([name], cipher) VALUES (@name, @cipher)";
        using var cmd = new SqlCommand(query, con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("name", SqlDbType.NVarChar, 30) {Value = publisher.Name});
        cmd.Parameters.Add(new SqlParameter("cipher", SqlDbType.NVarChar, 25) {Value = publisher.Cipher});
        return cmd.ExecuteNonQuery();
    }

    public static async Task<int> AddAsync(Publisher publisher)
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        const string query = "INSERT INTO Publishers ([name], cipher) VALUES (@name, @cipher)";
        await using var cmd = new SqlCommand(query, con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("name", SqlDbType.NVarChar, 30) {Value = publisher.Name});
        cmd.Parameters.Add(new SqlParameter("cipher", SqlDbType.NVarChar, 25) {Value = publisher.Cipher});
        return await cmd.ExecuteNonQueryAsync();
    }
}