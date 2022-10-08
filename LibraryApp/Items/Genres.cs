using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Genres
{
    public static IEnumerable<Genre> GetGenres()
    {
        using var con = new ConnectionDb();
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
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
}