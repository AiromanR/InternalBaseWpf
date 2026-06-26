using System.Windows;
using InternalBaseWpf.Service;

namespace InternalBaseWpf.Windows
{
    public partial class ActivistFilterWindow : Window
    {
        public ActivistFilter Filter { get; private set; } = new ActivistFilter();

        public ActivistFilterWindow(ActivistFilter? currentFilter = null)
        {
            InitializeComponent();
            LoadDictionaries();
            if (currentFilter != null)
            {
                Filter = currentFilter;
                FillForm();
            }
        }

        private void LoadDictionaries()
        {
            cmbGender.ItemsSource = new[] { "Мужской", "Женский", "Не определён" };
            cmbLoyalty.ItemsSource = new[] { "Лояльный, но не контактный", "Лояльность не подтверждена", "Лояльный" };
            cmbMainCell.ItemsSource = new CellService().GetAll();
        }

        private void FillForm()
        {
            txtLastName.Text = Filter.LastName ?? string.Empty;
            txtFirstName.Text = Filter.FirstName ?? string.Empty;
            txtPatronymic.Text = Filter.Patronymic ?? string.Empty;
            cmbGender.SelectedItem = Filter.Gender;
            txtPhone.Text = Filter.Phone ?? string.Empty;
            txtAddress.Text = Filter.Address ?? string.Empty;
            txtLocalBranch.Text = Filter.LocalBranch ?? string.Empty;
            txtPrimaryBranch.Text = Filter.PrimaryBranch ?? string.Empty;
            txtUikNumber.Text = Filter.UikNumber ?? string.Empty;
            cmbMainCell.SelectedValue = Filter.MainCellId;
            cmbLoyalty.SelectedItem = Filter.Loyalty;
            chkYoungerThan18.IsChecked = Filter.YoungerThan18;
            chkOlderThan100.IsChecked = Filter.OlderThan100;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            Filter = new ActivistFilter();
            DialogResult = true;
            Close();
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            Filter.LastName = txtLastName.Text.Trim();
            Filter.FirstName = txtFirstName.Text.Trim();
            Filter.Patronymic = txtPatronymic.Text.Trim();
            Filter.Gender = cmbGender.SelectedItem?.ToString();
            Filter.Phone = txtPhone.Text.Trim();
            Filter.Address = txtAddress.Text.Trim();
            Filter.LocalBranch = txtLocalBranch.Text.Trim();
            Filter.PrimaryBranch = txtPrimaryBranch.Text.Trim();
            Filter.UikNumber = txtUikNumber.Text.Trim();
            Filter.MainCellId = cmbMainCell.SelectedValue as int?;
            Filter.Loyalty = cmbLoyalty.SelectedItem?.ToString();
            Filter.YoungerThan18 = chkYoungerThan18.IsChecked == true;
            Filter.OlderThan100 = chkOlderThan100.IsChecked == true;

            DialogResult = true;
            Close();
        }
    }
}
