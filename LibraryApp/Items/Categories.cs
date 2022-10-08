using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Categories
{
    public static IEnumerable<Category> GetCategories()
    {
        using var con = new ConnectionDb();
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return GetCategory(reader);
        }
    }

    private static string GetString(string newQuery = "")
    {
        const string query = "SELECT * FROM [Categories]";
        return query + newQuery;
    }

    private static Category GetCategory(IDataRecord reader)
    {
        return new Category
        {
            Id = (int)reader["id"],
            Name = (string)reader["name"]
        };
    }
}