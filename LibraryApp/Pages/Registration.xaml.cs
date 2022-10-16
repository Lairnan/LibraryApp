using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LibraryApp.Items;
using LibraryApp.Models;
using LibraryApp.Windows;
using LibraryDialogForm;
using ComboBoxItem = LibraryDialogForm.ComboBoxItem;
using Type = LibraryApp.Models.Type;

namespace LibraryApp.Pages;

public partial class Registration : Page
{
    private PageService _pageService;
    public Registration(PageService pageService)
    {
        _pageService = pageService;
        if (MainWindow.Window.Height < 800)
        {
            MainWindow.Window.MinHeight = 800;
        }

        InitializeComponent();
        TypeBox.ItemsSource = Types;
        GetData();
        
        this.KeyDown += (s, e) =>
        {
            if (e.Key == Key.Enter) RegBtnClick(s, e);
        };
    }

    private async void GetData()
    {
        await foreach (var type in Items.Types.GetAsync())
        {
            if(type.Name != "Библиотекарь")
                Types.Add(type);
        }
        TypeBox.Items.Refresh();
        CaptchaGenerate();
    }

    private ObservableCollection<Models.Type> Types { get; set; } = new();


    private bool _isEnabled = false;
    
    private async void CaptchaGenerate()
    {
        CaptchaTextBox.Text = string.Empty;

        const string captcha = "1234567890" +
                               "abcdefghijklmnopqrstuvwxyz" +
                               "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        var newText = "";
        var rnd = new Random();
        for (var i = 0; i < 6; i++)
        {
            newText += captcha[rnd.Next(captcha.Length)];
        }

        CaptchaText.Text = newText;

        _isEnabled = true;
        await Task.Delay(15 * 1000);
        if (CaptchaText.Text == newText)
        {
            _isEnabled = false;
        }
    }
    
    private void UpdateCaptchaDown(object sender, MouseButtonEventArgs e)
    {
        CaptchaGenerate();
    }

    private bool CheckAttempts()
    {
        if (string.IsNullOrWhiteSpace(CaptchaTextBox.Text))
        {
            MessageBox.Show("Поля не могут быть пустыми");
            CaptchaGenerate();
            return false;
        }
            
        if (!_isEnabled)
        {
            MessageBox.Show("Время ввода капчи вышло");
            CaptchaGenerate();
            return false;
        }

        if (CaptchaTextBox.Text == CaptchaText.Text) return true;
        MessageBox.Show("Капча введена неверно");
        CaptchaGenerate();
        return false;
    }

    private async void RegBtnClick(object sender, RoutedEventArgs e)
    {
        if (!CheckAttempts()) return;
        
        try
        {
            if (GetValues(out var surname, 
                    out var name, 
                    out var type, 
                    out var group, 
                    out var groupName, 
                    out var birthdate, 
                    out var address, 
                    out var login, 
                    out var password, 
                    out var phone, 
                    out var patronymic, 
                    out var image, 
                    out var passport)) return;

            if (!IsNullOrWhiteSpace(surname, name, group, groupName, birthdate, address, login, password, phone))
            {
                var reader = GetReader(surname, name, patronymic, type, group, groupName, birthdate, address, login, password, phone, image);
                if (passport != null)
                {
                    var teacher = GetTeacher(reader, passport);
                    if (await CheckLog(teacher.Reader, teacher)) return;
                }

                if (await CheckLog(reader)) return;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        CaptchaGenerate();
    }

    private static Teacher GetTeacher(Reader reader, string passport)
    {
        return new Teacher
        {
            Reader = reader,
            Passport = passport
        };
    }

    private static Reader GetReader(string surname, string name, string? patronymic, 
        Type type, int group, string groupName, DateTime birthdate, 
        string address, string login, string password, long phone, string? image)
    {
        return new Reader 
        {
            Surname = surname,
            Name = name,
            Patronymic = patronymic,
            Type = type,
            Group = group,
            GroupName = groupName,
            BirthDate = birthdate,
            Address = address,
            Login = login,
            Password = password,
            Phone = phone,
            Image = image.ToByte()
        };
    }

    private static bool IsNullOrWhiteSpace(string surname, string name, int group, 
        string groupName, DateTime birthdate, string address, 
        string login, string password, long phone)
    {
        if (!(string.IsNullOrWhiteSpace(surname) | string.IsNullOrWhiteSpace(name) |
              string.IsNullOrWhiteSpace(group.ToString()) | string.IsNullOrWhiteSpace(groupName) |
              string.IsNullOrWhiteSpace(birthdate.ToString("dd.MM.yyyy")) |
              string.IsNullOrWhiteSpace(address) |
              string.IsNullOrWhiteSpace(login) |
              string.IsNullOrWhiteSpace(password) |
              string.IsNullOrWhiteSpace(phone.ToString()))) return false;
        MessageBox.Show("Поля не могут быть пустыми");
        return true;

    }

    private bool GetValues(out string surname, out string name, out Type type, out int group, out string groupName,
        out DateTime birthdate, out string address, out string login, out string password, out long phone,
        out string? patronymic, out string? image, out string? passport)
    {
        surname = SurBox.Text.Trim();
        name = NameBox.Text.Trim();
        type = Types[TypeBox.SelectedIndex];
        group = int.Parse(GroupBox.Text.Trim());
        groupName = ((TextBlock) GroupName.Items[GroupName.SelectedIndex]).Text.Trim();
        birthdate = DateTime.Parse(BirthBox.Text.Trim());
        address = AdrBox.Text.Trim();
        login = LogBox.Text.Trim();
        password = PassBox.Password.Trim();
        phone = long.Parse(PhoneBox.Text.Trim());
        patronymic = null;
        image = null;
        passport = null;
        if (!string.IsNullOrWhiteSpace(PatBox.Text))
        {
            patronymic = PatBox.Text;
        }

        if (!string.IsNullOrWhiteSpace(ImageBox.Text))
        {
            image = ImageBox.Text;
        }

        if (PassportBox.Visibility == Visibility.Visible)
        {
            passport = PassportBox.Text.Replace(' ', '\0');
            if (passport.Length != 10)
            {
                MessageBox.Show("Введите корректно серию и номер паспорта");
                return true;
            }
        }

        return false;
    }

    private async Task<bool> CheckLog(Reader reader, Teacher? teacher = null)
    {
        if (teacher != null)
            await Readers.AddTeacherAsync(teacher);
        else
            await Readers.AddAsync(reader);

        MessageBox.Show("Вы успешно зарегистрировались");
        _pageService.Start();
        return true;
    }

    private void AuthBtnClick(object sender, RoutedEventArgs e)
    {
        _pageService.Start();
    }

    private void TypeBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (TypeBox.SelectedIndex)
        {
            case 0:
                PassportBox.Visibility = Visibility.Collapsed;
                PassportLab.Visibility = Visibility.Collapsed;
                break;
            case 1:
                PassportBox.Visibility = Visibility.Visible;
                PassportLab.Visibility = Visibility.Visible;
                break;
        }
    }

    private void ImageBtn_OnClick(object sender, RoutedEventArgs e)
    {
        var file = new OpenFileDialogNew();
        file.FilterItems.Add(new ComboBoxItem
        {
            Name = "image",
            Filter = "*.jpg;*.jpeg;*.png;*.bmp"
        });
        file.ShowDialog();
        ImageBox.Text = file.FileName;
    }
}