using System.Linq;
using LibraryApp.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LibraryApp.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PageService _pageService;
        public MainWindow()
        {
            InitializeComponent();


            
            _pageService = new PageService();
            _pageService.OnPageChanged += page => ThisPage.Content = page;

            //this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth - 10;
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            Loaded += (_, _) =>
            {
                _pageService.Start();
                var windowMoves = new WindowMoves(this);
                this.MouseMove += windowMoves.DragMoveMouseMove;
                this.MouseLeftButtonDown += windowMoves.DragMoveLeftBtnDown;
                this.MouseLeftButtonUp += windowMoves.DragMoveLeftBtnUp;
            };
            Back = BtnBack;
            Exit = BtnExit;
            Window = this;
        }

        internal static Window Window = null!;
        internal static WindowState WindowChangeState = WindowState.Maximized;

        private void ExitBtnClick(object sender, RoutedEventArgs e)
        {
            _pageService.Start();
        }

        public static Button Back = null!;
        public static Button Exit = null!;

        private void BtnMin(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMinMax(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void BtnClose(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BackBtnClick(object sender, RoutedEventArgs e)
        {
            _pageService.GoToBack();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
    }
}