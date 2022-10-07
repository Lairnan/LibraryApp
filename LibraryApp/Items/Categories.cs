using System.Collections.Generic;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Categories
{
    public static IEnumerable<Category> GetCategories()
    {
        using var con = new ConnectionDb();
        const string query = "SELECT * FROM [Categories]";
        using var cmd = new SqlCommand(query, con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return new Category
            {
                Id = (int)reader["id"],
                Name = (string)reader["name"]
            };
        }
    }
}