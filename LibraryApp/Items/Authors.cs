using System.Collections.Generic;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Authors
{
    public static IEnumerable<Author> GetAuthors()
    {
        using var con = new ConnectionDb();
        const string query = "SELECT * FROM [Authors] a";
        using var cmd = new SqlCommand(query, con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return new Author
            {
                Id = (int)reader["id"],
                Surname = (string)reader["surname"],
                Name = (string)reader["name"],
                Patronymic = (string)reader["patronymic"],
                Cipher = (string)reader["cipher"]
            };
        }
    }
}