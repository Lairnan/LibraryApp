using System;

namespace LibraryApp.Models;

public class Book
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Author Author { get; set; }
    public Genre Genre { get; set; }
    public Category Category { get; set; }
    public Publisher Publisher { get; set; }
    public string UDC { get; set; }
    public string LBC { get; set; }
    public string ISBN { get; set; }
    public int Pages { get; set; }
    public City City { get; set; }
    public DateTime ReleaseDate { get; set; }
}