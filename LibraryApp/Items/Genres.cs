using System.Collections.Generic;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Genres
{
    public static IEnumerable<Genre> GetGenres()
    {
        using var con = new ConnectionDb();
        const string query = "SELECT * FROM [Genres]";
        using var cmd = new SqlCommand(query, con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return new Genre
            {
                Id = (int)reader["id"],
                Name = (string)reader["name"]
            };
        }
    }
}