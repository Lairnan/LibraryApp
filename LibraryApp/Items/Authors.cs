﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Authors
{
    public static IEnumerable<Author> GetAuthors()
    {
        using var con = new ConnectionDb();
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
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
}