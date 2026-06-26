using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace InternalBaseWpf.Windows
{
    public partial class ColumnSettingsWindow : Window
    {
        public List<ColumnSetting> Settings { get; private set; } = new();

        public ColumnSettingsWindow(List<ColumnSetting> settings)
        {
            InitializeComponent();
            Settings = settings.Select(s => new ColumnSetting { Header = s.Header, IsVisible = s.IsVisible }).ToList();
            lbColumns.ItemsSource = Settings;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }

    public class ColumnSetting
    {
        public string Header { get; set; } = null!;
        public bool IsVisible { get; set; } = true;
    }
}
