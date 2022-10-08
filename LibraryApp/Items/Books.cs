using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Books
{
    public static IEnumerable<Book> GetBooks()
    {
        using var con = ConnectionDb.ConnectionDbAsync().Result;
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return GetBook(reader);
        }
    }
    public static async IAsyncEnumerable<Book> GetBooksAsync()
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        await using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = await cmd.ExecuteReaderAsync();
        while (reader.Read())
        {
            yield return GetBook(reader);
        }
    }

    private static string GetString(string newQuery = "")
    {
        const string query = "SELECT b.*" +
                             ", a.id as authorId, a.surname as authorSurname, a.[name] as authorName, a.patronymic as authorPatronymic, a.cipher as authorCipher" +
                             ", cat.id as catId, cat.[name] as catName" +
                             ", g.id as gId, g.[name] as gName, p.id as pId, p.[name] as pName, p.cipher as pCipher" +
                             ", city.id as cityId, city.[name] as cityName"+
                             " FROM [Books] b" +
                             " INNER JOIN Authors a on a.id = b.authorId" +
                             " INNER JOIN Categories cat on cat.id = b.categoryId" +
                             " INNER JOIN Genres g on g.id = b.genreId" +
                             " INNER JOIN Publishers p on p.id = b.publisherId" +
                             " INNER JOIN Cities city on city.id = b.cityId";
        return query + newQuery;
    }

    private static Book GetBook(IDataRecord reader)
    {
        return new Book
        {
            Id = (int)reader["id"],
            Name = (string)reader["name"],
            Author = GetAuthor(reader),
            Category = GetCategory(reader),
            Genre = GetGenre(reader),
            Publisher = GetPublisher(reader),
            City = GetCity(reader),
            UDC = (string)reader["UDC"],
            LBC = (string)reader["LBC"],
            ISBN = (string)reader["ISBN"],
            Pages = (int)reader["pages"],
            ReleaseDate = (DateTime)reader["releaseDate"]
        };
    }

    private static Author GetAuthor(IDataRecord reader)
    {
        return new Author
        {
            Id = (int)reader["authorId"],
            Surname = (string)reader["authorSurname"],
            Name = (string)reader["authorName"],
            Patronymic = (string)reader["authorPatronymic"],
            Cipher = (string)reader["authorCipher"]
        };
    }

    private static City GetCity(IDataRecord reader)
    {
        return new City
        {
            Id = (int)reader["cityId"],
            Name = (string)reader["cityName"]
        };
    }

    private static Genre GetGenre(IDataRecord reader)
    {
        return new Genre
        {
            Id = (int)reader["gId"],
            Name = (string)reader["gName"]
        };
    }

    private static Category GetCategory(IDataRecord reader)
    {
        return new Category
        {
            Id = (int)reader["catId"],
            Name = (string)reader["catName"]
        };
    }

    private static Publisher GetPublisher(IDataRecord reader)
    {
        return new Publisher
        {
            Id = (int)reader["pId"],
            Name = (string)reader["pName"],
            Cipher = (string)reader["pCipher"]
        };
    }
}