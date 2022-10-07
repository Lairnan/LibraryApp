using System.Collections.Generic;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Publishers
{
    public static IEnumerable<Publisher> GetPublishers()
    {
        using var con = new ConnectionDb();
        const string query = "SELECT * FROM [Publishers]";
        using var cmd = new SqlCommand(query, con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return new Publisher
            {
                Id = (int)reader["id"],
                Name = (string)reader["name"],
                Cipher = (string)reader["cipher"]
            };
        }
    }
}