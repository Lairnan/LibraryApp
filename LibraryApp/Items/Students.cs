using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Students
{
    public static IEnumerable<Student> GetStudents()
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return GetStudent(reader);
        }
    }
    public static async IAsyncEnumerable<Student> GetStudentsAsync()
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        await using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = await cmd.ExecuteReaderAsync();
        while (reader.Read())
        {
            yield return GetStudent(reader);
        }
    }

    private static string GetString(string newQuery = "")
    {
        const string query = "SELECT r.*, t.[name], s.description as typeName FROM [Students] s" +
                             " INNER JOIN Readers r on r.id = s.readerId" +
                             " INNER JOIN Types T on T.id = r.typeid";
        return query + newQuery;
    }

    private static Student GetStudent(IDataRecord reader)
    {
        return new Student
        {
            Reader = GetReader(reader),
            Description = (string) reader["description"]
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