using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LibraryApp.Models;
using Type = LibraryApp.Models.Type;

namespace LibraryApp.Items;

public static class Loans
{
    public static IEnumerable<Loan> GetLoans()
    {
        using var con = new ConnectionDb();
        using var cmd = new SqlCommand(GetString(), con.SqlConnection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return GetLoan(reader);
        }
    }

    private static string GetString(string newQuery = "")
    {
        const string query = "SELECT r.id as readerId" +
                             ", r.surname as readerSurname, r.[name] as readerName, r.patronymic as readerPatronymic" +
                             ", t.id as typeId, t.[name] as typeName" +
                             ", r.address, r.birthdate, r.[login], r.[password], r.phone" +
                             ", b.*" +
                             ", a.id as authorId, a.surname as authorSurname, a.[name] as authorName, a.patronymic as authorPatronymic" +
                             ", a.cipher as authorCipher" +
                             ", cat.id as catId, cat.[name] as catName" +
                             ", g.id as gId, g.[name] as gName, p.id as pId, p.[name] as pName, p.cipher as pCipher" +
                             ", city.id as cityId, city.[name] as cityName"+
                             " FROM [Loans] l" +
                             " INNER JOIN Readers r on r.id = l.readerId" +
                             " INNER JOIN Books b on b.id = l.bookId" +
                             " INNER JOIN [Types] t on t.id = r.typeid" +
                             " INNER JOIN Authors a on a.id = b.authorId" +
                             " INNER JOIN Categories cat on cat.id = b.categoryId" +
                             " INNER JOIN Genres g on g.id = b.genreId" +
                             " INNER JOIN Publishers p on p.id = b.publisherId" +
                             " INNER JOIN Cities city on city.id = b.cityId";
        return query + newQuery;
    }

    private static Loan GetLoan(IDataRecord reader)
    {
        return new Loan
            {
                Book = GetBook(reader),
                Reader = GetReader(reader),
                TakeDate = (DateTime)reader["takeDate"],
                PlanDateDel = (DateTime)reader["planDateDel"],
                FactDateDel = (DateTime)reader["factDateDel"]
            };
    }

    private static Reader GetReader(IDataRecord reader)
    {
        return new Reader
        {
            Id = (int) reader["readerId"],
            Surname = (string) reader["readerSurname"],
            Name = (string) reader["readerName"],
            Patronymic = (string) reader["readerPatronymic"],
            Type = new Type
            {
                Id = (int) reader["typeid"],
                Name = (string) reader["typeName"]
            },
            Address = (string) reader["address"],
            Phone = (long) reader["phone"],
            Login = (string) reader["login"],
            Password = (string) reader["password"]
        };
    }

    private static Book GetBook(IDataRecord reader)
    {
        return new Book
        {
            Id = (int) reader["id"],
            Name = (string) reader["name"],
            Author = new Author
            {
                Id = (int) reader["authorId"],
                Surname = (string) reader["authorSurname"],
                Name = (string) reader["authorName"],
                Patronymic = (string) reader["authorPatronymic"],
                Cipher = (string) reader["authorCipher"]
            },
            Category = new Category
            {
                Id = (int) reader["catId"],
                Name = (string) reader["catName"]
            },
            Genre = new Genre
            {
                Id = (int) reader["gId"],
                Name = (string) reader["gName"]
            },
            Publisher = new Publisher
            {
                Id = (int) reader["pId"],
                Name = (string) reader["pName"],
                Cipher = (string) reader["pCipher"]
            },
            City = new City
            {
                Id = (int) reader["cityId"],
                Name = (string) reader["cityName"]
            },
            UDC = (string) reader["UDC"],
            LBC = (string) reader["LBC"],
            ISBN = (string) reader["ISBN"],
            Pages = (int) reader["pages"],
            ReleaseDate = (DateTime) reader["releaseDate"]
        };
    }
}