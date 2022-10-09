using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Genres
{
    public static IEnumerable<Genre> Get()
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return GetGenre(reader);
        }
    }
    public static async IAsyncEnumerable<Genre> GetAsync()
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        await using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = await cmd.ExecuteReaderAsync();
        while (reader.Read())
        {
            yield return GetGenre(reader);
        }
    }

    private static string GetString(string newQuery = "")
    {
        const string query = "SELECT * FROM [Genres]";
        return query + newQuery;
    }

    private static Genre GetGenre(IDataRecord reader)
    {
        return new Genre
        {
            Id = (int)reader["id"],
            Name = (string)reader["name"]
        };
    }

    public static int Add(Genre genre)
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        const string query = "INSERT INTO Genres ([name]) VALUES (@name)";
        using var cmd = new SqlCommand(query, con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("name", SqlDbType.NVarChar, 30) {Value = genre.Name});
        return cmd.ExecuteNonQuery();
    }

    public static async Task<int> AddAsync(Genre genre)
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        const string query = "INSERT INTO Genres ([name]) VALUES (@name)";
        await using var cmd = new SqlCommand(query, con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("name", SqlDbType.NVarChar, 30) {Value = genre.Name});
        return await cmd.ExecuteNonQueryAsync();
    }
}