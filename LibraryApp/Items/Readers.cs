using System.Collections.Generic;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Readers
{
    public static IEnumerable<Reader> GetReaders()
    {
        using var con = new ConnectionDb();
        const string query = "SELECT r.*, t.[name] as typeName FROM [Readers] r" +
                             " INNER JOIN Types T on T.id = r.typeid";
        using var cmd = new SqlCommand(query, con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return new Reader
            {
                Id = (int) reader["id"],
                Surname = (string) reader["surname"],
                Name = (string) reader["name"],
                Patronymic = (string) reader["patronymic"],
                Type = new Type
                {
                    Id = (int)reader["typeid"],
                    Name = (string)reader["typeName"]
                },
                Address = (string)reader["address"],
                Phone = (long)reader["phone"],
                Login = (string)reader["login"],
                Password = (string)reader["password"]
            };
        }
    }
}