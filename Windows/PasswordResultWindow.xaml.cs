using System.Windows;

namespace InternalBaseWpf.Windows
{
    public partial class PasswordResultWindow : Window
    {
        public string Password { get; }
        public string Header { get; }

        public PasswordResultWindow(string password, string title = "Пароль создан", string header = "ПОЛЬЗОВАТЕЛЬ СОЗДАН")
        {
            Password = password;
            Header = header;
            DataContext = this;
            InitializeComponent();
            Title = title;
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Password);
            MessageBox.Show("Пароль скопирован в буфер обмена", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
