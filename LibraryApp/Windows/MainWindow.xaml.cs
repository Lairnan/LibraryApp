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
        private PageService PageService = null!;
        public MainWindow()
        {
            InitializeComponent();


            
            PageService = new PageService();
            PageService.OnPageChanged += page => ThisPage.Content = page;

            //this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth - 10;
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            Loaded += (_, _) =>
            {
                PageService.Start();
                var windowMoves = new WindowMoves(this);
                this.MouseMove += windowMoves.DragMoveMouseMove;
                this.MouseLeftButtonDown += windowMoves.DragMoveLeftBtnDown;
                this.MouseLeftButtonUp += windowMoves.DragMoveLeftBtnUp;
            };
            Back = BtnBack;
        }

        public static Button Back = null!;

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
            PageService.GoToBack();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
    }
}