using System.Collections.Generic;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Types
{
    public static IEnumerable<Type> GetTypes()
    {
        using var con = new ConnectionDb();
        const string query = "SELECT * FROM [Types]";
        using var cmd = new SqlCommand(query, con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return new Type
            {
                Id = (int) reader["id"],
                Name = (string) reader["name"]
            };
        }
    }
}