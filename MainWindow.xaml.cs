using System.Windows;
using InternalBaseWpf.Pages;

namespace InternalBaseWpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new LoginPage());
        }
    }
}
