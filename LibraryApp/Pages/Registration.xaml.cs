using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LibraryApp.Items;
using LibraryApp.Models;
using LibraryApp.Windows;

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
            var surname = SurBox.Text.Trim();
            var name = NameBox.Text.Trim();
            var patronymic = PatBox.Text.Trim();
            var type = Types[TypeBox.SelectedIndex];
            var group = int.Parse(GroupBox.Text.Trim());
            var groupName = ((TextBlock)GroupName.Items[GroupName.SelectedIndex]).Text.Trim();
            var birthdate = DateTime.Parse(BirthBox.Text.Trim());
            var address = AdrBox.Text.Trim();
            var login = LogBox.Text.Trim();
            var password = PassBox.Password.Trim();
            var phone = long.Parse(PhoneBox.Text.Trim());

            /*MessageBox.Show(groupName);
            MessageBox.Show(birthdate.ToString("dd.MM.yyyy"));
            MessageBox.Show(phone.ToString());
            MessageBox.Show(type.Name);*/

            /*MessageBox.Show($"Surname - {surname} \nName - {name} \nPatronymic - {patronymic} " +
                            $"\nType - {type.Id} - {type.Name} \nGroup - {group} " +
                            $"\nGroupName - {groupName} \nBirthdate - {birthdate:dd.MM.yyyy} " +
                            $"\nAddress - {address} \nLogin - {login} \nPassword - {password} " +
                            $"\nPhone - {phone}");*/

            if (string.IsNullOrWhiteSpace(surname) | string.IsNullOrWhiteSpace(name) | string.IsNullOrWhiteSpace(patronymic) | 
                string.IsNullOrWhiteSpace(group.ToString()) | string.IsNullOrWhiteSpace(groupName) | 
                string.IsNullOrWhiteSpace(birthdate.ToString("dd.MM.yyyy")) | 
                string.IsNullOrWhiteSpace(address) | 
                string.IsNullOrWhiteSpace(login) | 
                string.IsNullOrWhiteSpace(password) |
                string.IsNullOrWhiteSpace(phone.ToString()))
            {
                MessageBox.Show("Поля не могут быть пустыми");
            }
            else
            {
                var reader = new Reader
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
                    Phone = phone
                };
                if (await CheckLog(reader)) return;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        CaptchaGenerate();
    }

    private async Task<bool> CheckLog(Reader reader)
    {
        using var con = await ConnectionDb.ConnectionDbAsync();
        await using var cmd =
            new SqlCommand("SELECT * FROM [Readers] WHERE (surname = @surname AND [name] = @name AND patronymic = @patronymic)" +
                           " OR (login = @login)",
                con.SqlConnection);
            
        cmd.Parameters.AddWithValue("surname", reader.Surname);
        cmd.Parameters.AddWithValue("name", reader.Name);
        cmd.Parameters.AddWithValue("patronymic", reader.Patronymic);
        cmd.Parameters.AddWithValue("login", reader.Login);
        cmd.Parameters.AddWithValue("password", reader.Password);
            
        var read = await cmd.ExecuteReaderAsync();
        if (await read.ReadAsync())
        {
            MessageBox.Show("Данный пользователь уже существует");
        }

        await Readers.AddAsync(reader);
        MessageBox.Show("Вы успешно зарегистрировались");
        _pageService.Start();
        return true;
    }

    private void AuthBtnClick(object sender, RoutedEventArgs e)
    {
        _pageService.Start();
    }
}