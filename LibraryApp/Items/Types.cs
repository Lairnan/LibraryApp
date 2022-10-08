﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Types
{
    public static IEnumerable<Type> GetTypes()
    {
        using var con = new ConnectionDb();
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return GetType(reader);
        }
    }

    private static string GetString(string newQuery = "")
    {
        const string query = "SELECT * FROM [Types]";
        return query + newQuery;
    }

    private static Type GetType(IDataRecord reader)
    {
        return new Type
        {
            Id = (int) reader["id"],
            Name = (string) reader["name"]
        };
    }
}