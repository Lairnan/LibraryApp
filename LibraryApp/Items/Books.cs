using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using LibraryApp.Models;

namespace LibraryApp.Items;

public static class Books
{
    public static IEnumerable<Book> GetBooks()
    {
        using var con = new ConnectionDb();
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
        using var cmd = new SqlCommand(query, con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return new Book
            {
                Id = (int)reader["id"],
                Name = (string)reader["name"],
                Author = new Author
                {
                    Id = (int)reader["authorId"],
                    Surname = (string)reader["authorSurname"],
                    Name = (string)reader["authorName"],
                    Patronymic = (string)reader["authorPatronymic"],
                    Cipher = (string)reader["authorCipher"]
                },
                Category = new Category
                {
                    Id = (int)reader["catId"],
                    Name = (string)reader["catName"]
                },
                Genre = new Genre
                {
                    Id = (int)reader["gId"],
                    Name = (string)reader["gName"]
                },
                Publisher = new Publisher
                {
                    Id = (int)reader["pId"],
                    Name = (string)reader["pName"],
                    Cipher = (string)reader["pCipher"]
                },
                City = new City
                {
                    Id = (int)reader["cityId"],
                    Name = (string)reader["cityName"]
                },
                UDC = (string)reader["UDC"],
                LBC = (string)reader["LBC"],
                ISBN = (string)reader["ISBN"],
                Pages = (int)reader["pages"],
                ReleaseDate = (DateTime)reader["releaseDate"]
            };
        }
    }
}