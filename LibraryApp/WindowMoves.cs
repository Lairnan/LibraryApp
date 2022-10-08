using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LibraryApp.Windows;

namespace LibraryApp;

public class WindowMoves
{
    public WindowMoves(Window window)
    {
        this._window = window;
    }

    private readonly Window _window;

    private bool _mRestoreIfMove = false;

    public void DragMoveLeftBtnDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            if (_window.ResizeMode is ResizeMode.CanResize or ResizeMode.CanResizeWithGrip)
            {
                SwitchState();
            }

            return;
        }

        if (_window.WindowState == WindowState.Maximized)
        {
            _mRestoreIfMove = true;
            return;
        }

        _window.DragMove();
    }
    
    public void DragMoveLeftBtnUp(object sender, MouseButtonEventArgs e)
    {
        _mRestoreIfMove = false;
    }
    
    public void DragMoveMouseMove(object sender, MouseEventArgs e)
    {
        if (!_mRestoreIfMove) return;
        _mRestoreIfMove = false;

        var percentHorizontal = e.GetPosition(_window).X / _window.ActualWidth;
        var targetHorizontal = _window.RestoreBounds.Width * percentHorizontal;
        
        var percentVertical = e.GetPosition(_window).Y / _window.ActualHeight;
        var targetVertical = _window.RestoreBounds.Height * percentVertical;


        var point = _window.PointToScreen(e.MouseDevice.GetPosition(_window));

        _window.Left = point.X - targetHorizontal;
        _window.Top = point.Y - targetVertical;
        
        _window.WindowState = WindowState.Normal;

        _window.DragMove();
    }

    private void SwitchState()
    {
        switch (_window)
        {
            case {WindowState: WindowState.Normal}:
                MainWindow.WindowChangeState = WindowState.Maximized;
                _window.WindowState = WindowState.Maximized;
                break;
            case {WindowState: WindowState.Maximized}:
                MainWindow.WindowChangeState = WindowState.Normal;
                _window.WindowState = WindowState.Normal;
                break;
            default:
                MainWindow.WindowChangeState = _window.WindowState;
                _window.WindowState = _window.WindowState;
                break;
        }
    }
    
    public void BtnMin(object sender, RoutedEventArgs e)
    {
        _window.WindowState = WindowState.Minimized;
    }

    public void BtnMinMax(object sender, RoutedEventArgs e)
    {
        _window.WindowState = _window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    public void BtnClose(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}