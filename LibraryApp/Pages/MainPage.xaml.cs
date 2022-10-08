using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
        }

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
