using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Authors
{
    public static IEnumerable<Author> Get()
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return GetAuthor(reader);
        }
    }
    public static async IAsyncEnumerable<Author> GetAsync()
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        await using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = await cmd.ExecuteReaderAsync();
        while (reader.Read())
        {
            yield return GetAuthor(reader);
        }
    }

    private static string GetString(string newQuery = "")
    {
        const string query = "SELECT * FROM [Authors] a";
        return query + newQuery;
    }

    private static Author GetAuthor(IDataRecord reader)
    {
        return new Author
        {
            Id = (int)reader["id"],
            Surname = (string)reader["surname"],
            Name = (string)reader["name"],
            Patronymic = (string)reader["patronymic"],
            Cipher = (string)reader["cipher"]
        };
    }

    public static int Add(Author author)
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        const string query = "INSERT INTO Authors (surname, [name], patronymic, cipher) VALUES (@surname, @name, @patronymic, @cipher)";
        using var cmd = new SqlCommand(query, con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("surname", SqlDbType.NVarChar, 30) {Value = author.Surname});
        cmd.Parameters.Add(new SqlParameter("name", SqlDbType.NVarChar, 30) {Value = author.Name});
        cmd.Parameters.Add(new SqlParameter("patronymic", SqlDbType.NVarChar, 30) {Value = author.Patronymic});
        cmd.Parameters.Add(new SqlParameter("cipher", SqlDbType.NVarChar, 5) {Value = author.Cipher});
        return cmd.ExecuteNonQuery();
    }

    public static async Task<int> AddAsync(Author author)
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        const string query = "INSERT INTO Authors (surname, [name], patronymic, cipher) VALUES (@surname, @name, @patronymic, @cipher)";
        await using var cmd = new SqlCommand(query, con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("surname", SqlDbType.NVarChar, 30) {Value = author.Surname});
        cmd.Parameters.Add(new SqlParameter("name", SqlDbType.NVarChar, 30) {Value = author.Name});
        cmd.Parameters.Add(new SqlParameter("patronymic", SqlDbType.NVarChar, 30) {Value = author.Patronymic});
        cmd.Parameters.Add(new SqlParameter("cipher", SqlDbType.NVarChar, 5) {Value = author.Cipher});
        return await cmd.ExecuteNonQueryAsync();
    }
}