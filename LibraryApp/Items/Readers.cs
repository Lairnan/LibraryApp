using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using LibraryApp.Models;
using Type = LibraryApp.Models.Type;

namespace LibraryApp.Items;

public static class Readers
{
    public static IEnumerable<Reader> Get()
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return GetReader(reader);
        }
    }
    public static async IAsyncEnumerable<Reader> GetAsync()
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        await using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = await cmd.ExecuteReaderAsync();
        while (reader.Read())
        {
            yield return GetReader(reader);
        }
    }

    private static string GetString(string newQuery = "")
    {
        const string query = "SELECT r.*, t.[name] as typeName, gn.name as gnName FROM [Readers] r" +
                             " INNER JOIN Types T on T.id = r.typeid" +
                             " INNER JOIN GroupsName gn on r.groupNameId = gn.id";
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
            Group = (int)reader["group"],
            GroupName = (string)reader["gnName"],
            BirthDate = (DateTime)reader["birthdate"],
            Address = (string)reader["address"],
            Phone = (long)reader["phone"],
            Login = (string)reader["login"],
            Password = (string)reader["password"],
            Passport = (string)reader["passport"]
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

    public static int Add(Reader reader)
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        const string query = "INSERT INTO Readers (surname, [name], patronymic, typeid, [group], " +
                             "groupName, birthdate, [address], [login], [password], phone, passport) " +
                             "VALUES " +
                             "(@surname, @name, @patronymic, @type, @group, @groupName, @birthdate, @address, @login, @password, @phone, @passport)";
        using var cmd = new SqlCommand(query, con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("surname", SqlDbType.NVarChar, 30) {Value = reader.Surname});
        cmd.Parameters.Add(new SqlParameter("name", SqlDbType.NVarChar, 30) {Value = reader.Name});
        cmd.Parameters.Add(new SqlParameter("patronymic", SqlDbType.NVarChar, 30) {Value = reader.Patronymic});
        cmd.Parameters.AddWithValue("type", reader.Type.Id);
        cmd.Parameters.AddWithValue("group", reader.Group);
        cmd.Parameters.Add(new SqlParameter("groupName", SqlDbType.NVarChar, 1) {Value = reader.GroupName});
        cmd.Parameters.AddWithValue("birthdate", reader.BirthDate);
        cmd.Parameters.Add(new SqlParameter("address", SqlDbType.NVarChar, 50) {Value = reader.Address});
        cmd.Parameters.Add(new SqlParameter("login", SqlDbType.NVarChar, 50) {Value = reader.Login});
        cmd.Parameters.Add(new SqlParameter("password", SqlDbType.NVarChar, 50) {Value = reader.Password});
        cmd.Parameters.AddWithValue("phone", reader.Phone);
        cmd.Parameters.Add(new SqlParameter("passport", SqlDbType.NVarChar, 10) {Value = reader.Passport});
        return cmd.ExecuteNonQuery();
    }

    public static async Task<int> AddAsync(Reader reader)
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        const string query = "INSERT INTO Readers (surname, [name], patronymic, typeid, [group], " +
                             "groupName, birthdate, [address], [login], [password], phone, passport) " +
                             "VALUES " +
                             "(@surname, @name, @patronymic, @type, @group, @groupName, @birthdate, @address, @login, @password, @phone, @passport)";
        await using var cmd = new SqlCommand(query, con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("surname", SqlDbType.NVarChar, 30) {Value = reader.Surname});
        cmd.Parameters.Add(new SqlParameter("name", SqlDbType.NVarChar, 30) {Value = reader.Name});
        cmd.Parameters.Add(new SqlParameter("patronymic", SqlDbType.NVarChar, 30) {Value = reader.Patronymic});
        cmd.Parameters.AddWithValue("type", reader.Type.Id);
        cmd.Parameters.AddWithValue("group", reader.Group);
        cmd.Parameters.Add(new SqlParameter("groupName", SqlDbType.NVarChar, 1) {Value = reader.GroupName});
        cmd.Parameters.AddWithValue("birthdate", reader.BirthDate);
        cmd.Parameters.Add(new SqlParameter("address", SqlDbType.NVarChar, 50) {Value = reader.Address});
        cmd.Parameters.Add(new SqlParameter("login", SqlDbType.NVarChar, 50) {Value = reader.Login});
        cmd.Parameters.Add(new SqlParameter("password", SqlDbType.NVarChar, 50) {Value = reader.Password});
        cmd.Parameters.AddWithValue("phone", reader.Phone);
        cmd.Parameters.Add(new SqlParameter("passport", SqlDbType.NVarChar, 10) {Value = reader.Passport});
        return await cmd.ExecuteNonQueryAsync();
    }
}