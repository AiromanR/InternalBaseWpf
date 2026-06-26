using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InternalBaseWpf.Models;
using InternalBaseWpf.Service;
using InternalBaseWpf.Windows;

namespace InternalBaseWpf.Pages
{
    public partial class AdminPage : Page
    {
        private readonly UserService _service = new();
        private List<UserViewModel> _allData = new();
        private List<UserViewModel> _currentPageData = new();
        private int _currentPage = 1;
        private int _pageSize = 25;

        public AdminPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            _allData = _service.GetAll()
                .Select(u => new UserViewModel(u))
                .ToList();
            _currentPage = 1;
            ApplyPagination();
        }

        private void ApplyPagination()
        {
            int total = _allData.Count;
            int totalPages = (int)System.Math.Ceiling((double)total / _pageSize);
            if (totalPages < 1)
                totalPages = 1;
            if (_currentPage > totalPages)
                _currentPage = totalPages;
            if (_currentPage < 1)
                _currentPage = 1;

            _currentPageData = _allData
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            dgUsers.ItemsSource = _currentPageData;
            int from = (_currentPage - 1) * _pageSize + 1;
            int to = System.Math.Min(_currentPage * _pageSize, total);
            txtStatus?.SetCurrentValue(TextBlock.TextProperty, $"Показаны строки с {from} по {to} из {total}");
            txtPageInfo?.SetCurrentValue(TextBlock.TextProperty, $"{_currentPage} / {totalPages}");
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var loginDialog = new InputDialog("Введите логин:", "Добавление пользователя");
            if (loginDialog.ShowDialog() != true)
                return;
            string login = loginDialog.ResponseText;
            if (string.IsNullOrWhiteSpace(login))
                return;

            var nameDialog = new InputDialog("Введите ФИО:", "Добавление пользователя");
            if (nameDialog.ShowDialog() != true)
                return;
            string fullName = nameDialog.ResponseText;
            if (string.IsNullOrWhiteSpace(fullName))
                return;

            var result = MessageBox.Show("Назначить пользователя администратором?", "Роль", MessageBoxButton.YesNo, MessageBoxImage.Question);
            bool isAdmin = result == MessageBoxResult.Yes;

            try
            {
                _service.Create(login, fullName, isAdmin, out string generatedPassword);
                var passwordWindow = new PasswordResultWindow(generatedPassword);
                passwordWindow.ShowDialog();
                LoadData();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnToggleAdmin_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem is not UserViewModel vm)
                return;

            _service.ToggleAdmin(vm.Id);
            LoadData();
        }

        private void BtnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem is not UserViewModel vm)
                return;

            string newPassword = _service.ResetPassword(vm.Id);
            var passwordWindow = new PasswordResultWindow(newPassword, "Пароль изменён", "ПАРОЛЬ ИЗМЕНЁН");
            passwordWindow.ShowDialog();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem is not UserViewModel vm)
                return;

            if (MessageBox.Show($"Удалить пользователя {vm.FullName}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            _service.Delete(vm.Id);
            LoadData();
        }

        private void CmbPageSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPageSize.SelectedItem is ComboBoxItem item && int.TryParse(item.Content.ToString(), out int size))
            {
                _pageSize = size;
                _currentPage = 1;
                ApplyPagination();
            }
        }

        private void BtnPage_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
                return;

            int totalPages = (int)System.Math.Ceiling((double)_allData.Count / _pageSize);
            if (totalPages < 1)
                totalPages = 1;

            _currentPage = btn.Tag?.ToString() switch
            {
                "First" => 1,
                "Prev" => System.Math.Max(1, _currentPage - 1),
                "Next" => System.Math.Min(totalPages, _currentPage + 1),
                "Last" => totalPages,
                _ => _currentPage
            };

            ApplyPagination();
        }
    }

    public class UserViewModel
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string RoleText { get; set; } = null!;
        public bool IsActive { get; set; }
        public System.DateTime CreatedAt { get; set; }

        public UserViewModel(User user)
        {
            Id = user.Id;
            Login = user.Login;
            FullName = user.FullName;
            RoleText = user.IsAdmin ? "Администратор" : "Пользователь";
            IsActive = user.IsActive;
            CreatedAt = user.CreatedAt;
        }
    }
}
