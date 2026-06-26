using System.Linq;
using System.Windows;
using InternalBaseWpf.Models;
using InternalBaseWpf.Service;

namespace InternalBaseWpf.Windows
{
    public partial class ActivistEditWindow : Window
    {
        private readonly ActivistService _service = new();
        private readonly Activist? _activist;

        public ActivistEditWindow(Activist? activist = null)
        {
            InitializeComponent();
            _activist = activist;
            LoadDictionaries();
            if (_activist != null)
                FillForm();
        }

        private void LoadDictionaries()
        {
            cmbGender.ItemsSource = new[] { "Мужской", "Женский", "Не определён" };
            cmbPartyStatus.ItemsSource = new[] { "беспартийный", "член партии", "сторонник" };
            cmbOrgStatus.ItemsSource = new[] { "Не указан", "Работник", "Волонтёр" };
            cmbLoyalty.ItemsSource = new[] { "Лояльный, но не контактный", "Лояльность не подтверждена", "Лояльный" };

            cmbResponsibleUser.ItemsSource = new UserService().GetAll();
            cmbMainCell.ItemsSource = new CellService().GetAll();
        }

        private void FillForm()
        {
            if (_activist == null)
                return;

            txtLastName.Text = _activist.LastName;
            txtFirstName.Text = _activist.FirstName;
            txtPatronymic.Text = _activist.Patronymic ?? string.Empty;
            cmbGender.SelectedItem = _activist.Gender;
            dpBirthDate.SelectedDate = _activist.BirthDate;
            txtPhone.Text = _activist.Phone ?? string.Empty;
            txtAddress.Text = _activist.Address ?? string.Empty;
            txtLocalBranch.Text = _activist.LocalBranch ?? string.Empty;
            txtPrimaryBranch.Text = _activist.PrimaryBranch ?? string.Empty;
            txtUikNumber.Text = _activist.UikNumber ?? string.Empty;
            chkIsAts.IsChecked = _activist.IsAts;
            chkIsVverh.IsChecked = _activist.IsVverh;

            chkIsResponsible.IsChecked = _activist.IsResponsible;
            chkCanEdit.IsChecked = _activist.CanEdit;
            cmbResponsibleUser.SelectedValue = _activist.ResponsibleUserId;
            cmbMainCell.SelectedValue = _activist.MainCellId;
            cmbPartyStatus.SelectedItem = _activist.PartyStatus;
            cmbOrgStatus.SelectedItem = _activist.OrgStatus;
            txtWorkplace.Text = _activist.Workplace ?? string.Empty;
            txtPosition.Text = _activist.Position ?? string.Empty;
            txtNote.Text = _activist.Note ?? string.Empty;
            chkPartySupporter.IsChecked = _activist.IsPartySupporter;
            chkPresidentSupporter.IsChecked = _activist.IsPresidentSupporter;
            chkVdlSupporter.IsChecked = _activist.IsVdlSupporter;
            cmbLoyalty.SelectedItem = _activist.Loyalty;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text) || string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Введите фамилию и имя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string phone = txtPhone.Text.Trim();
            if (!string.IsNullOrWhiteSpace(phone) && _service.PhoneExists(phone, _activist?.Id))
            {
                MessageBox.Show("Указанный мобильный телефон уже используется", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var activist = _activist ?? new Activist();
            activist.LastName = txtLastName.Text.Trim();
            activist.FirstName = txtFirstName.Text.Trim();
            activist.Patronymic = txtPatronymic.Text.Trim();
            activist.Gender = cmbGender.SelectedItem?.ToString();
            activist.BirthDate = dpBirthDate.SelectedDate;
            activist.Phone = txtPhone.Text.Trim();
            activist.Address = txtAddress.Text.Trim();
            activist.LocalBranch = txtLocalBranch.Text.Trim();
            activist.PrimaryBranch = txtPrimaryBranch.Text.Trim();
            activist.UikNumber = txtUikNumber.Text.Trim();
            activist.IsAts = chkIsAts.IsChecked == true;
            activist.IsVverh = chkIsVverh.IsChecked == true;

            activist.IsResponsible = chkIsResponsible.IsChecked == true;
            activist.CanEdit = chkCanEdit.IsChecked == true;
            activist.ResponsibleUserId = (int?)cmbResponsibleUser.SelectedValue;
            activist.MainCellId = (int?)cmbMainCell.SelectedValue;
            activist.PartyStatus = cmbPartyStatus.SelectedItem?.ToString();
            activist.OrgStatus = cmbOrgStatus.SelectedItem?.ToString();
            activist.Workplace = txtWorkplace.Text.Trim();
            activist.Position = txtPosition.Text.Trim();
            activist.Note = txtNote.Text.Trim();
            activist.IsPartySupporter = chkPartySupporter.IsChecked == true;
            activist.IsPresidentSupporter = chkPresidentSupporter.IsChecked == true;
            activist.IsVdlSupporter = chkVdlSupporter.IsChecked == true;
            activist.Loyalty = cmbLoyalty.SelectedItem?.ToString();

            if (_activist == null)
                _service.Add(activist);
            else
                _service.Update(activist);

            DialogResult = true;
            Close();
        }
    }
}
