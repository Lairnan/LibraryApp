using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Teachers
{
    public static IEnumerable<Teacher> Get()
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return GetTeacher(reader);
        }
    }
    public static async IAsyncEnumerable<Teacher> GetAsync()
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        await using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = await cmd.ExecuteReaderAsync();
        while (reader.Read())
        {
            yield return GetTeacher(reader);
        }
    }

    private static string GetString(string newQuery = "")
    {
        const string query = "SELECT r.*, t.[name], s.passport as typeName FROM [Teachers] s" +
                             " INNER JOIN Readers r on r.id = s.readerId" +
                             " INNER JOIN Types T on T.id = r.typeid";
        return query + newQuery;
    }

    private static Teacher GetTeacher(IDataRecord reader)
    {
        return new Teacher
        {
            Reader = GetReader(reader),
            Passport = (string)reader["passport"]
        };
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
            Address = (string) reader["address"],
            Phone = (long) reader["phone"],
            Login = (string) reader["login"],
            Password = (string) reader["password"]
        };
    }

    private static Type GetType(IDataRecord reader)
    {
        return new Type
        {
            Id = (int) reader["typeid"],
            Name = (string) reader["typeName"]
        };
    }
}