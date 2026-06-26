using System.Windows;

namespace InternalBaseWpf.Windows
{
    public partial class InputDialog : Window
    {
        public string ResponseText { get; private set; } = string.Empty;

        public InputDialog(string prompt, string title = "Ввод", string defaultValue = "")
        {
            InitializeComponent();
            Title = title;
            txtPrompt.Text = prompt;
            txtValue.Text = defaultValue;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            ResponseText = txtValue.Text;
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
