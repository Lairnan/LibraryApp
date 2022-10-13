using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using LibraryApp.Items;
using LibraryApp.Models;

namespace LibraryApp.Pages.Adds;

public partial class AddBook : Page
{
    private PageService _pageService;
    public AddBook(PageService pageService)
    {
        _pageService = pageService;
        InitializeComponent();
        
        GenreBox.ItemsSource = GenresCollection;
        AuthorBox.ItemsSource = AuthorsCollection;
        CategoryBox.ItemsSource = CategoriesCollection;
        PubBox.ItemsSource = PublishersCollection;
        CityBox.ItemsSource = CitiesCollection;
        GetData();
    }

    private async void GetData()
    {
        Parallel.Invoke(GetAuthors,
            GetGenres,
            GetCategories,
            GetPublishers,
            GetCities);
    }

    private async void GetCities()
    {
        await foreach (var item in Cities.GetAsync())
        {
            CitiesCollection.Add(item);
        }
    }

    private async void GetPublishers()
    {
        await foreach (var item in Publishers.GetAsync())
        {
            PublishersCollection.Add(item);
        }
    }

    private async void GetCategories()
    {
        await foreach (var item in Categories.GetAsync())
        {
            CategoriesCollection.Add(item);
        }
    }

    private async void GetGenres()
    {
        await foreach (var item in Genres.GetAsync())
        {
            GenresCollection.Add(item);
        }
    }

    private async void GetAuthors()
    {
        await foreach (var item in Authors.GetAsync())
        {
            AuthorsCollection.Add(item);
        }
    }

    private ObservableCollection<Author> AuthorsCollection { get; set; } = new();
    private ObservableCollection<Genre> GenresCollection { get; set; } = new();
    private ObservableCollection<Category> CategoriesCollection { get; set; } = new();
    private ObservableCollection<Publisher> PublishersCollection { get; set; } = new();
    private ObservableCollection<City> CitiesCollection { get; set; } = new();
}