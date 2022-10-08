using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Readers
{
    public static IEnumerable<Reader> GetReaders()
    {
        using var con = new ConnectionDb();
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return GetReader(reader);
        }
    }

    private static string GetString(string newQuery = "")
    {
        const string query = "SELECT r.*, t.[name] as typeName FROM [Readers] r" +
                             " INNER JOIN Types T on T.id = r.typeid";
        return query + newQuery;
    }

    private static Reader GetReader(IDataRecord reader)
    {
        return new Reader
        {
            Id = (int) reader["id"],
            Surname = (string) reader["surname"],
            Name = (string) reader["name"],
            Patronymic = (string) reader["patronymic"],
            Type = GetType(reader),
            Address = (string)reader["address"],
            Phone = (long)reader["phone"],
            Login = (string)reader["login"],
            Password = (string)reader["password"]
        };
    }

    private static Type GetType(IDataRecord reader)
    {
        return new Type
        {
            Id = (int)reader["typeid"],
            Name = (string)reader["typeName"]
        };
    }
}