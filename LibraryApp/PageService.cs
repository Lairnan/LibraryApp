﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace LibraryApp;

public class PageService
{
    private readonly Stack<Page> _history;
    public bool CanGoToBack => _history.Skip(1).Any();
    
    public event Action<Page>? OnPageChanged;

    public PageService()
    {
        _history = new Stack<Page>();
    }

    public void Navigate(Page page)
    {
        OnPageChanged?.Invoke(page);
        
        _history.Push(page);

        MainWindow.Back.IsEnabled = CanGoToBack;
    }

    public void GoToBack()
    {
        _history.Pop();
        OnPageChanged?.Invoke(_history.Peek());
        MainWindow.Back.IsEnabled = CanGoToBack;
    }
}