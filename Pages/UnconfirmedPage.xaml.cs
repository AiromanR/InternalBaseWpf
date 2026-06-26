using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InternalBaseWpf.Models;
using InternalBaseWpf.Service;
using Microsoft.Win32;

namespace InternalBaseWpf.Pages
{
    public partial class UnconfirmedPage : Page
    {
        private readonly ActivistService _service = new();
        private readonly CellService _cellService = new();
        private List<UnconfirmedViewModel> _allItems = new();
        private List<UnconfirmedViewModel> _items = new();
        private List<UnconfirmedViewModel> _currentPageData = new();
        private int _currentPage = 1;
        private int _pageSize = 25;

        public UnconfirmedPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFilters();
            LoadData();
        }

        private void LoadFilters()
        {
            cmbMainCell.ItemsSource = _cellService.GetAll();
            cmbMainCell.DisplayMemberPath = "Name";
            cmbMainCell.SelectedValuePath = "Id";

            cmbCellFilter.ItemsSource = _cellService.GetAll();
            cmbCellFilter.DisplayMemberPath = "Name";
            cmbCellFilter.SelectedValuePath = "Id";
        }

        private void LoadData()
        {
            var activists = _service.GetUnconfirmed();
            _allItems = activists.Select(a => new UnconfirmedViewModel(a, _service.PhoneExists(a.Phone, a.Id))).ToList();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var query = _allItems.AsEnumerable();

            string search = txtSearch.Text.Trim();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var lower = search.ToLower();
                query = query.Where(vm =>
                    vm.FullName.ToLower().Contains(lower) ||
                    (vm.Phone != null && vm.Phone.ToLower().Contains(lower)));
            }

            if (cmbMainCell.SelectedValue is int mainCellId)
                query = query.Where(vm => vm.MainCellId == mainCellId);

            if (cmbCellFilter.SelectedValue is int cellId)
                query = query.Where(vm => vm.CellIds.Contains(cellId));

            _items = query.ToList();
            _currentPage = 1;
            ApplyPagination();
        }

        private void ApplyPagination()
        {
            int total = _items.Count;
            int totalPages = (int)System.Math.Ceiling((double)total / _pageSize);
            if (totalPages < 1)
                totalPages = 1;
            if (_currentPage > totalPages)
                _currentPage = totalPages;
            if (_currentPage < 1)
                _currentPage = 1;

            _currentPageData = _items
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            dgUnconfirmed.ItemsSource = _currentPageData;
            int from = (_currentPage - 1) * _pageSize + 1;
            int to = System.Math.Min(_currentPage * _pageSize, total);
            txtStatus?.SetCurrentValue(TextBlock.TextProperty, $"Показаны строки с {from} по {to} из {total}");
            txtPageInfo?.SetCurrentValue(TextBlock.TextProperty, $"{_currentPage} / {totalPages}");
        }

        private void BtnTemplate_Click(object sender, RoutedEventArgs e)
        {
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "ActivistImportTemplate.xlsx");
            Directory.CreateDirectory(Path.GetDirectoryName(templatePath)!);
            ExcelService.CreateTemplate(templatePath);
            Process.Start(new ProcessStartInfo(templatePath) { UseShellExecute = true });
        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Excel файлы (*.xlsx)|*.xlsx",
                Title = "Импорт активистов"
            };

            if (dialog.ShowDialog() != true)
                return;

            var imported = ExcelService.ImportFromExcel(dialog.FileName, out var errors);
            foreach (var activist in imported)
            {
                _service.Add(activist);
            }

            if (errors.Any())
            {
                MessageBox.Show(string.Join("\n", errors), "Ошибки импорта", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show($"Импортировано {imported.Count} записей", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            LoadData();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgUnconfirmed.SelectedItem is not UnconfirmedViewModel vm)
                return;

            var activist = _service.GetById(vm.Id);
            if (activist == null)
                return;

            var window = new Windows.ActivistEditWindow(activist);
            if (window.ShowDialog() == true)
                LoadData();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (dgUnconfirmed.SelectedItem is not UnconfirmedViewModel vm)
                return;

            if (vm.HasPhoneConflict)
            {
                MessageBox.Show("Невозможно подтвердить запись с конфликтом телефона", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _service.Confirm(vm.Id);
            LoadData();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgUnconfirmed.SelectedItem is not UnconfirmedViewModel vm)
                return;

            if (MessageBox.Show($"Удалить запись \"{vm.FullName}\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            _service.Delete(vm.Id);
            LoadData();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
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

            int totalPages = (int)System.Math.Ceiling((double)_items.Count / _pageSize);
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

    public class UnconfirmedViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? LocalBranch { get; set; }
        public string BirthDateText { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
        public int? MainCellId { get; set; }
        public List<int> CellIds { get; set; } = new();
        public bool HasPhoneConflict { get; set; }
        public string ImportStatus => HasPhoneConflict ? "Моб. телефон уже используется" : "Готово к импорту";

        public UnconfirmedViewModel(Activist activist, bool hasPhoneConflict)
        {
            Id = activist.Id;
            FullName = activist.FullName;
            LocalBranch = activist.LocalBranch;
            BirthDateText = activist.BirthDate?.ToString("dd.MM.yyyy") ?? string.Empty;
            Email = activist.Email;
            Phone = activist.Phone;
            Address = activist.Address;
            Note = activist.Note;
            MainCellId = activist.MainCellId;
            CellIds = activist.ActivistCells.Select(ac => ac.CellId).ToList();
            HasPhoneConflict = hasPhoneConflict;
        }
    }
}
