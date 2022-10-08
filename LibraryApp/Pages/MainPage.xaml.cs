using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LibraryApp.Items;
using LibraryApp.Models;

namespace LibraryApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private readonly PageService _pageService;
        public MainPage(PageService pageService)
        {
            _pageService = pageService;
            InitializeComponent();
            GetDates();
            Lv.ItemsSource = Readers;
        }

        private async void GetDates()
        {
            await foreach (var reader in Items.Readers.GetReadersAsync())
            {
                Readers.Add(reader);
            }
            Lv.Items.Refresh();
        }

        private ObservableCollection<Reader> Readers { get; } = new();

        private void ExitBtnClick(object sender, RoutedEventArgs e)
        {
            _pageService.Start();
        }

        private void NextBtnClick(object sender, RoutedEventArgs e)
        {
            _pageService.Navigate(new MainPage(_pageService));
        }
    }
}
