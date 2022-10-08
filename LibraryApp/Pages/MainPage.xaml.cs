﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LibraryApp.Items;
using LibraryApp.Models;
using LibraryApp.Pages.Views;

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

        private void ShowBooksBtnClick(object sender, RoutedEventArgs e)
        {
            _pageService.Navigate(new BooksView(_pageService));
        }
    }
}
