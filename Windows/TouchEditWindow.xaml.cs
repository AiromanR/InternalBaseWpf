using System;
using System.Windows;
using InternalBaseWpf.Models;
using InternalBaseWpf.Service;

namespace InternalBaseWpf.Windows
{
    public partial class TouchEditWindow : Window
    {
        private readonly TouchService _service = new();
        private readonly Touch? _touch;

        public TouchEditWindow(Touch? touch = null)
        {
            InitializeComponent();
            _touch = touch;
            LoadDictionaries();
            if (_touch != null)
                FillForm();
        }

        private void LoadDictionaries()
        {
            cmbCell.ItemsSource = new CellService().GetAll();
            cmbType.ItemsSource = _service.GetTypes();
        }

        private void FillForm()
        {
            if (_touch == null)
                return;

            txtName.Text = _touch.Name;
            txtDescription.Text = _touch.Description ?? string.Empty;
            txtMediaLink.Text = _touch.MediaLink ?? string.Empty;
            txtSocialLink.Text = _touch.SocialLink ?? string.Empty;
            txtCoverage.Text = _touch.Coverage?.ToString() ?? string.Empty;
            dpStartDate.SelectedDate = _touch.StartDate;
            dpEndDate.SelectedDate = _touch.EndDate;
            cmbCell.SelectedValue = _touch.CellId;
            cmbType.SelectedValue = _touch.TypeId;
            chkIsActive.IsChecked = _touch.IsActive;
            chkShopPlaced.IsChecked = _touch.IsShopPlaced;
            chkRikPlaced.IsChecked = _touch.IsRikPlaced;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название касания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var touch = _touch ?? new Touch();
            touch.Name = txtName.Text.Trim();
            touch.Description = txtDescription.Text.Trim();
            touch.MediaLink = txtMediaLink.Text.Trim();
            touch.SocialLink = txtSocialLink.Text.Trim();
            touch.Coverage = int.TryParse(txtCoverage.Text.Trim(), out int coverage) ? coverage : null;
            touch.StartDate = dpStartDate.SelectedDate;
            touch.EndDate = dpEndDate.SelectedDate;
            touch.CellId = (int?)cmbCell.SelectedValue;
            touch.TypeId = (int?)cmbType.SelectedValue;
            touch.IsActive = chkIsActive.IsChecked == true;
            touch.IsShopPlaced = chkShopPlaced.IsChecked == true;
            touch.IsRikPlaced = chkRikPlaced.IsChecked == true;

            if (_touch == null && Application.Current.Properties["CurrentUser"] is Models.User user)
                touch.OperatorId = user.Id;

            if (_touch == null)
                _service.Add(touch);
            else
                _service.Update(touch);

            DialogResult = true;
            Close();
        }
    }
}
