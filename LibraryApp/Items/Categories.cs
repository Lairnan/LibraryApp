﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Categories
{
    public static IEnumerable<Category> Get()
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return GetCategory(reader);
        }
    }
    public static async IAsyncEnumerable<Category> GetAsync()
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        await using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = await cmd.ExecuteReaderAsync();
        while (reader.Read())
        {
            yield return GetCategory(reader);
        }
    }

    private static string GetString(string newQuery = "")
    {
        const string query = "SELECT * FROM [Categories]";
        return query + newQuery;
    }

    private static Category GetCategory(IDataRecord reader)
    {
        return new Category
        {
            Id = (int)reader["id"],
            Name = (string)reader["name"]
        };
    }

    public static int Add(Category category)
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        const string query = "INSERT INTO Categories ([name]) VALUES (@name)";
        using var cmd = new SqlCommand(query, con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("name", SqlDbType.NVarChar, 30) {Value = category.Name});
        return cmd.ExecuteNonQuery();
    }

    public static async Task<int> AddAsync(Category category)
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        const string query = "INSERT INTO Categories ([name]) VALUES (@name)";
        await using var cmd = new SqlCommand(query, con.SqlConnection);
        cmd.Parameters.Add(new SqlParameter("name", SqlDbType.NVarChar, 30) {Value = category.Name});
        return await cmd.ExecuteNonQueryAsync();
    }
}