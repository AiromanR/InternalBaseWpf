using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using InternalBaseWpf.Models;

namespace InternalBaseWpf.Pages
{
    public partial class MainMenuPage : Page
    {
        private User? _currentUser;
        private Button? _activeButton;

        public MainMenuPage()
        {
            InitializeComponent();
            DataContext = this;
            Application.Current.MainWindow.Title = "Внутренняя база";

            _currentUser = Application.Current.Properties["CurrentUser"] as User;
            txtUserName.Text = _currentUser?.FullName ?? "Пользователь";

            if (_currentUser?.IsAdmin == true)
                btnAdmin.Visibility = Visibility.Visible;

            NavigateTo("Activists");
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                NavigateTo(btn.Tag?.ToString() ?? string.Empty);
            }
        }

        private void NavigateTo(string tag)
        {
            Page? page = tag switch
            {
                "Touches" => new TouchesPage(),
                "Activists" => new ActivistsPage(),
                "Cells" => new CellsPage(),
                "Unconfirmed" => new UnconfirmedPage(),
                "Duplicates" => new DuplicatesPage(),
                "Admin" => new AdminPage(),
                _ => null
            };

            if (page != null)
            {
                ContentFrame.Navigate(page);
                HighlightButton(tag);
            }
        }

        private void HighlightButton(string tag)
        {
            _activeButton?.SetValue(ForegroundProperty, FindResource("MutedTextBrush"));

            _activeButton = tag switch
            {
                "Touches" => btnTouches,
                "Activists" => btnActivists,
                "Cells" => btnCells,
                "Unconfirmed" => btnUnconfirmed,
                "Duplicates" => btnDuplicates,
                "Admin" => btnAdmin,
                _ => null
            };

            _activeButton?.SetValue(ForegroundProperty, FindResource("PrimaryBlue"));
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["CurrentUser"] = null;
            NavigationService?.Navigate(new LoginPage());
        }
    }
}
