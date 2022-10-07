using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace LibraryApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        private PageService _pageService;

        public Authorization(PageService pageService)
        {
            this._pageService = pageService;
            InitializeComponent();
            
            ListUsers = TestUsers.GetUsersCollection().ToList();
        }

        private List<Users> ListUsers;

        private int _attemps = 0;

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

        private void AuthBtnClick(object sender, RoutedEventArgs e)
        {
            if (_attemps > 3)
            {
                if (!_isEnabled)
                {
                    MessageBox.Show("Время ввода капчи вышло");
                    CaptchaGenerate();
                    return;
                }
                if (CaptchaTextBox.Text != CaptchaText.Text)
                {
                    MessageBox.Show("Капча введена неверно");
                    CaptchaGenerate();
                    return;
                }
            }

            var login = LogBox.Text;
            var password = PassBox.Password;
            if (string.IsNullOrWhiteSpace(login) | string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Поля не могут быть пустыми");
            }
            else
            {
                if (ListUsers.Any(s => s.Login == login))
                {
                    if (ListUsers.Any(s => s.Password == password))
                    {
                        _pageService.Enter();
                        return;
                    }

                    MessageBox.Show("Неверный пароль");
                }
                else
                {
                    MessageBox.Show("Неверный логин");
                }
            }

            if (++_attemps > 2)
            {
                CaptchaGenerate();
            }
        }
        private void UpdateCaptchaDown(object sender, MouseButtonEventArgs e)
        {
            CaptchaGenerate();
        }

        private void RegBtnClick(object sender, RoutedEventArgs e){}
    }
}
