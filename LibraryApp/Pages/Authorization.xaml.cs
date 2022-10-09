using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LibraryApp.Windows;

namespace LibraryApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        private readonly PageService _pageService;

        public Authorization(PageService pageService)
        {
            this._pageService = pageService;
            MainWindow.Window.MinHeight = 350;
            MainWindow.Window.Height = 450;
            InitializeComponent();

            this.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter) AuthBtnClick(s, e);
            };
        }

        private int _attempts = 0;

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
            CaptchaText.Visibility = Visibility.Visible;
            CaptchaTextBox.Visibility = Visibility.Visible;
            CaptchaUpdate.Visibility = Visibility.Visible;

            _isEnabled = true;
            await Task.Delay(15 * 1000);
            if (CaptchaText.Text == newText)
            {
                _isEnabled = false;
            }
        }

        private bool CheckAttempts()
        {
            if (_attempts < 3) return true;
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

        private async Task<bool> CheckLog(string login, string password)
        {
            using var con = await ConnectionDb.ConnectionDbAsync();
            await using var cmd =
                new SqlCommand("SELECT * FROM [Readers] WHERE login = @login AND password = @password",
                    con.SqlConnection);
            
            cmd.Parameters.AddWithValue("login", login);
            cmd.Parameters.AddWithValue("password", password);
            
            var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                _pageService.Enter();
                return true;
            }

            MessageBox.Show("Неверный логин или пароль");
            return false;
        }

        private async void AuthBtnClick(object sender, RoutedEventArgs e)
        {
            if (!CheckAttempts()) return;

            var login = LogBox.Text;
            var password = PassBox.Password;
            if (string.IsNullOrWhiteSpace(login) | string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Поля не могут быть пустыми");
            }
            else
            {
                if (await CheckLog(login, password)) return;
            }

            if (++_attempts > 2)
            {
                CaptchaGenerate();
            }
        }
        
        private void UpdateCaptchaDown(object sender, MouseButtonEventArgs e)
        {
            CaptchaGenerate();
        }

        private void RegBtnClick(object sender, RoutedEventArgs e)
        {
            _pageService.Reg();
        }
    }
}
