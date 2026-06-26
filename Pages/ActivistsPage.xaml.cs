using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InternalBaseWpf.Models;
using InternalBaseWpf.Service;
using InternalBaseWpf.Windows;
using Microsoft.Win32;

namespace InternalBaseWpf.Pages
{
    public partial class ActivistsPage : Page
    {
        private readonly ActivistService _service = new();
        private ActivistFilter _filter = new();
        private List<ActivistViewModel> _allData = new();
        private List<ActivistViewModel> _currentPageData = new();
        private List<ColumnSetting> _columnSettings = new();
        private int _currentPage = 1;
        private int _pageSize = 25;

        public ActivistsPage()
        {
            InitializeComponent();
            InitializeColumnSettings();
        }

        private void InitializeColumnSettings()
        {
            var saved = SettingsService.LoadColumnSettings("Activists");
            if (saved != null && saved.Count == 9)
            {
                _columnSettings = saved;
                return;
            }

            _columnSettings = new List<ColumnSetting>
            {
                new ColumnSetting { Header = "Местное отделение" },
                new ColumnSetting { Header = "ФИО" },
                new ColumnSetting { Header = "Дата рождения" },
                new ColumnSetting { Header = "Моб. телефон" },
                new ColumnSetting { Header = "Ответственный" },
                new ColumnSetting { Header = "УИК №" },
                new ColumnSetting { Header = "Основная ячейка" },
                new ColumnSetting { Header = "Ячейки" },
                new ColumnSetting { Header = "Дата создания" }
            };
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFilters();
            ApplyFilter();
        }

        private void LoadFilters()
        {
            cmbMainCell.ItemsSource = new CellService().GetAll();
            cmbMainCell.DisplayMemberPath = "Name";
            cmbMainCell.SelectedValuePath = "Id";
        }

        private void ApplyFilter()
        {
            _filter.SearchText = txtSearch.Text.Trim();
            _filter.YoungerThan18 = chkYoungerThan18.IsChecked == true;
            _filter.OlderThan100 = chkOlderThan100.IsChecked == true;
            _filter.MainCellId = cmbMainCell.SelectedValue as int?;

            var activists = _service.Filter(_filter);
            _allData = activists.Select(a => new ActivistViewModel(a)).ToList();
            _currentPage = 1;
            ApplyPagination();
            ApplyColumnVisibility();
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

            dgActivists.ItemsSource = _currentPageData;
            int from = (_currentPage - 1) * _pageSize + 1;
            int to = System.Math.Min(_currentPage * _pageSize, total);
            txtStatus?.SetCurrentValue(TextBlock.TextProperty, $"Показаны строки с {from} по {to} из {total}");
            txtPageInfo?.SetCurrentValue(TextBlock.TextProperty, $"{_currentPage} / {totalPages}");
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

        private void ApplyColumnVisibility()
        {
            var columns = new Dictionary<string, DataGridColumn>
            {
                ["Местное отделение"] = colLocalBranch,
                ["ФИО"] = colFullName,
                ["Дата рождения"] = colBirthDate,
                ["Моб. телефон"] = colPhone,
                ["Ответственный"] = colResponsible,
                ["УИК №"] = colUik,
                ["Основная ячейка"] = colMainCell,
                ["Ячейки"] = colCells,
                ["Дата создания"] = colCreatedAt
            };

            foreach (var setting in _columnSettings)
            {
                if (columns.TryGetValue(setting.Header, out var column))
                    column.Visibility = setting.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void QuickFilter_Checked(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = new ActivistEditWindow();
            if (window.ShowDialog() == true)
                ApplyFilter();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgActivists.SelectedItem is not ActivistViewModel vm)
                return;

            var activist = _service.GetById(vm.Id);
            if (activist == null)
                return;

            var window = new ActivistEditWindow(activist);
            if (window.ShowDialog() == true)
                ApplyFilter();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgActivists.SelectedItem is not ActivistViewModel vm)
                return;

            if (MessageBox.Show($"Удалить активиста \"{vm.FullName}\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            _service.Delete(vm.Id);
            ApplyFilter();
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Excel файлы (*.xlsx)|*.xlsx",
                Title = "Экспорт активистов",
                FileName = "Активисты.xlsx"
            };

            if (dialog.ShowDialog() != true)
                return;

            ExcelService.ExportActivists(dialog.FileName, _allData.Select(vm => vm.Source).ToList());
            MessageBox.Show("Экспорт завершён", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnFilters_Click(object sender, RoutedEventArgs e)
        {
            var window = new ActivistFilterWindow(_filter);
            if (window.ShowDialog() == true)
            {
                _filter = window.Filter;
                SyncToolbarWithFilter();
                ApplyFilter();
            }
        }

        private void SyncToolbarWithFilter()
        {
            txtSearch.Text = _filter.SearchText ?? string.Empty;
            chkYoungerThan18.IsChecked = _filter.YoungerThan18;
            chkOlderThan100.IsChecked = _filter.OlderThan100;
            cmbMainCell.SelectedValue = _filter.MainCellId;
        }

        private void BtnColumns_Click(object sender, RoutedEventArgs e)
        {
            var window = new ColumnSettingsWindow(_columnSettings);
            if (window.ShowDialog() == true)
            {
                _columnSettings = window.Settings;
                SettingsService.SaveColumnSettings("Activists", _columnSettings);
                ApplyColumnVisibility();
            }
        }
    }

    public class ActivistViewModel
    {
        public Activist Source { get; }
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? LocalBranch { get; set; }
        public string BirthDateText { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string ResponsibleName { get; set; } = string.Empty;
        public string? UikNumber { get; set; }
        public string MainCellName { get; set; } = string.Empty;
        public string CellsText { get; set; } = string.Empty;
        public string CreatedAtText { get; set; } = string.Empty;

        public ActivistViewModel(Activist activist)
        {
            Source = activist;
            Id = activist.Id;
            FullName = activist.FullName;
            LocalBranch = activist.LocalBranch;
            BirthDateText = activist.BirthDate?.ToString("dd.MM.yyyy") ?? string.Empty;
            Phone = activist.Phone;
            ResponsibleName = activist.ResponsibleUser?.FullName ?? (activist.IsResponsible ? "Да" : "Нет");
            UikNumber = activist.UikNumber;
            MainCellName = activist.MainCell?.Name ?? string.Empty;
            CellsText = string.Join(", ", activist.ActivistCells.Select(ac => ac.Cell.Name));
            CreatedAtText = activist.CreatedAt.ToString("dd.MM.yyyy HH:mm");
        }
    }
}
