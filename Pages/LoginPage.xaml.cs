using System.Windows;
using System.Windows.Controls;
using InternalBaseWpf.Models;
using InternalBaseWpf.Service;

namespace InternalBaseWpf.Pages
{
    public partial class LoginPage : Page
    {
        private readonly UserService _userService = new();

        public LoginPage()
        {
            InitializeComponent();
            DataContext = this;
            Application.Current.MainWindow.Title = "Вход в систему";
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            User? user = _userService.Authenticate(login, password);
            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Application.Current.Properties["CurrentUser"] = user;
            NavigationService?.Navigate(new MainMenuPage());
        }
    }
}
