using System;

namespace LibraryApp.Models;

public class Loan
{
    public Reader Reader { get; set; }
    public Book Book { get; set; }
    public DateTime TakeDate { get; set; }
    public DateTime PlanDateDel { get; set; }
    public DateTime? FactDateDel { get; set; }
}