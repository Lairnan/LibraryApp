using System;
using System.Windows;
using System.Windows.Controls;

namespace LibraryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static PageService PageService = null!;
        public MainWindow()
        {
            InitializeComponent();
            
            Loaded += (_, _) =>
            {
                PageService.Navigate(new TestPage(PageService));
                var windowMoves = new WindowMoves(this);
                this.MouseLeftButtonDown += windowMoves.DragMoveLeftBtnDown;
                this.MouseLeftButtonUp += windowMoves.DragMoveLeftBtnUp;
                this.MouseMove += windowMoves.DragMoveMouseMove;
            };
        }

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
    }
}