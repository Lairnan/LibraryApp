using System;

namespace LibraryApp.Models;

public class Reader
{
    public int Id { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string? Patronymic { get; set; }
    public Type Type { get; set; }
    public DateTime BirthDate { get; set; }
    public string Address { get; set; }
    public int Phone { get; set; }
}