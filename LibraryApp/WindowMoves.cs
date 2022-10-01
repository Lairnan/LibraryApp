﻿using System.Windows;
using System.Windows.Input;

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
        _window.WindowState = _window.WindowState switch
        {
            WindowState.Normal => WindowState.Maximized,
            WindowState.Maximized => WindowState.Normal,
            _ => _window.WindowState
        };
    }
}