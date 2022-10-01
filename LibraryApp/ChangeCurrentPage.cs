using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace LibraryApp;

public class ChangeCurrentPage : INotifyPropertyChanged
{
    private readonly PageService _pageService;
    public ChangeCurrentPage()
    {
        _pageService = new PageService();
        _pageService.OnPageChanged += page => CurrentPage = page;
        MainWindow.PageService = this._pageService;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private Page _currentPage;

    public Page CurrentPage
    {
        get => _currentPage;
        private set
        {
            _currentPage = value; 
            OnPropertyChanged();
        }
    }
}