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
        const string query = "SELECT r.*, t.[name] as typeName, te.passport " +
                             "FROM [Readers] r " +
                             "INNER JOIN Types T on T.id = r.typeid " +
                             "LEFT JOIN Teachers te on r.id = te.readerid ";
        return query + newQuery + " order by id";
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
            Group = (int) reader["group"],
            GroupName = (string) reader["groupName"],
            BirthDate = (DateTime) reader["birthdate"],
            Address = (string) reader["address"],
            Phone = (long) reader["phone"],
            Login = (string) reader["login"],
            Password = (string) reader["password"],
            Image = (byte[]) reader["image"],
            Teacher = GetTeacher(reader)
        };
    }

    private static Teacher GetTeacher(IDataRecord teacher)
    {
        return new Teacher
        {
            // Reader = GetReader(teacher),
            Passport = (string) teacher["passport"],
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
        CheckLogin(reader).Wait();
        CheckPhone(reader).Wait();
        var cmd = GetCommand(con).Result;
        cmd.Parameters["@surname"].Value = reader.Surname;
        cmd.Parameters["@name"].Value = reader.Name;
        cmd.Parameters["@patronymic"].Value = string.IsNullOrWhiteSpace(reader.Patronymic) 
                ? DBNull.Value 
                : reader.Patronymic;
        cmd.Parameters["@typeid"].Value = reader.Type.Id;
        cmd.Parameters["@group"].Value = reader.Group;
        cmd.Parameters["@groupName"].Value = reader.GroupName;
        cmd.Parameters["@birthdate"].Value = reader.BirthDate;
        cmd.Parameters["@address"].Value = reader.Address;
        cmd.Parameters["@login"].Value = reader.Login;
        cmd.Parameters["@password"].Value = reader.Password;
        cmd.Parameters["@phone"].Value = reader.Phone;
        cmd.Parameters["@passport"].Value = string.IsNullOrWhiteSpace(reader.Passport) 
            ? DBNull.Value 
            : reader.Passport;
        cmd.Parameters["@image"].Value = reader.Image == null 
            ? DBNull.Value 
            : reader.Image;
        return cmd.ExecuteNonQuery();
    }

    private static async Task<SqlCommand> GetCommand(ConnectionDb con)
    {
        const string query = "INSERT INTO Readers " +
                             "(surname, [name], patronymic, typeid, [group], " +
                             "groupName, birthdate, [address], [login], [password], phone, [image]) " +
                             "VALUES " +
                             "(@surname, @name, @patronymic, @typeid, @group, " +
                             "@groupName, @birthdate, @address, @login, @password, @phone, @image)";
        await using var cmd = new SqlCommand(query, con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("@surname", SqlDbType.NVarChar, 30));
        cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 30));
        cmd.Parameters.Add(new SqlParameter("@patronymic", SqlDbType.NVarChar, 30));
        cmd.Parameters.Add(new SqlParameter("@typeid", SqlDbType.Int));
        cmd.Parameters.Add(new SqlParameter("@group", SqlDbType.Int));
        cmd.Parameters.Add(new SqlParameter("@groupName", SqlDbType.NVarChar, 1));
        cmd.Parameters.Add(new SqlParameter("@birthdate", SqlDbType.Date));
        cmd.Parameters.Add(new SqlParameter("@address", SqlDbType.NVarChar, 50));
        cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.NVarChar, 50));
        cmd.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar, 50));
        cmd.Parameters.Add(new SqlParameter("@phone", SqlDbType.BigInt));
        cmd.Parameters.Add(new SqlParameter("@passport", SqlDbType.NVarChar, 10));
        cmd.Parameters.Add(new SqlParameter("@image", SqlDbType.Image));
        return cmd;
    }

    public static async Task<int> AddTeacherAsync(Teacher teacher)
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        await CheckPassport(con, teacher);
        await AddAsync(teacher.Reader);
        int readerId;
        await using (var readerCmd = new SqlCommand(GetString("where login = @login"),
                         con.SqlConnection))
        {
            readerCmd.Parameters.AddWithValue("@login", teacher.Reader.Login);
            readerId = (int) readerCmd.ExecuteScalar();
            readerCmd.Dispose();
        }

        await using var cmd = new SqlCommand("INSERT INTO Teachers VALUES (@readerId, @passport)", 
            con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("@readerId", SqlDbType.Int)).Value = readerId;
        cmd.Parameters.Add(new SqlParameter("@passport", SqlDbType.NVarChar, 10)).Value = 
            string.IsNullOrWhiteSpace(teacher.Passport) 
            ? DBNull.Value 
            : teacher.Passport;
        return await cmd.ExecuteNonQueryAsync();
    }

    private static async Task CheckPassport(ConnectionDb con, Teacher teacher)
    {
        await using var cmd =
            new SqlCommand("SELECT * FROM [Teachers] WHERE (passport = @passport)",
                con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("@passport", SqlDbType.NVarChar, 10) {Value = teacher.Passport});
        var read = await cmd.ExecuteReaderAsync();
        if (read.Read())
        {
            throw new Exception("Паспорт не может повторяться");
        }
    }

    private static async Task CheckLogin(Reader reader)
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        await using var cmd =
            new SqlCommand("SELECT * FROM [Readers] WHERE (login = @login)",
                con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.NVarChar, 50) {Value = reader.Login});
        var read = await cmd.ExecuteReaderAsync();
        if (read.Read())
        {
            throw new Exception("Такой логин уже существует");
        }
    }

    private static async Task CheckPhone(Reader reader)
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        await using var cmd =
            new SqlCommand("SELECT * FROM [Readers] WHERE (phone = @phone)",
                con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("@phone", SqlDbType.BigInt) {Value = reader.Phone});
        var read = await cmd.ExecuteReaderAsync();
        if (read.Read())
        {
            throw new Exception("Номер телефона уже существует");
        }
    }

    public static async Task<int> AddAsync(Reader reader)
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        await CheckLogin(reader);
        await CheckPhone(reader);
        var cmd = await GetCommand(con);
        cmd.Parameters["@surname"].Value = reader.Surname;
        cmd.Parameters["@name"].Value = reader.Name;
        cmd.Parameters["@patronymic"].Value = string.IsNullOrWhiteSpace(reader.Patronymic) 
            ? DBNull.Value 
            : reader.Patronymic;
        cmd.Parameters["@typeid"].Value = reader.Type.Id;
        cmd.Parameters["@group"].Value = reader.Group;
        cmd.Parameters["@groupName"].Value = reader.GroupName;
        cmd.Parameters["@birthdate"].Value = reader.BirthDate;
        cmd.Parameters["@address"].Value = reader.Address;
        cmd.Parameters["@login"].Value = reader.Login;
        cmd.Parameters["@password"].Value = reader.Password;
        cmd.Parameters["@phone"].Value = reader.Phone;
        cmd.Parameters["@passport"].Value = string.IsNullOrWhiteSpace(reader.Passport) 
            ? DBNull.Value 
            : reader.Passport;
        cmd.Parameters["@image"].Value = reader.Image == null 
            ? DBNull.Value 
            : reader.Image;
        return await cmd.ExecuteNonQueryAsync();
    }
}