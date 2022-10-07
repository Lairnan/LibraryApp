using System.Collections.Generic;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Cities
{
    public static IEnumerable<City> GetCities()
    {
        using var con = new ConnectionDb();
        const string query = "SELECT * FROM [Cities]";
        using var cmd = new SqlCommand(query, con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return new City
            {
                Id = (int)reader["id"],
                Name = (string)reader["name"]
            };
        }
    }
}