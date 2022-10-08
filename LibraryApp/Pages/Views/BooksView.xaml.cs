using System.Collections.ObjectModel;
using System.Windows.Controls;
using LibraryApp.Models;

namespace LibraryApp.Pages.Views;

public partial class BooksView : Page
{
    private PageService _pageService;
    public BooksView(PageService pageService)
    {
        _pageService = pageService;
        InitializeComponent();
        GetDates();
        Lv.ItemsSource = Books;
    }

    private async void GetDates()
    {
        await foreach (var book in Items.Books.GetAsync())
        {
            Books.Add(book);
        }
        Lv.Items.Refresh();
    }

    private ObservableCollection<Book> Books { get; } = new();
}