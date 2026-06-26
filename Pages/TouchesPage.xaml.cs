using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InternalBaseWpf.Models;
using InternalBaseWpf.Service;
using InternalBaseWpf.Windows;

namespace InternalBaseWpf.Pages
{
    public partial class TouchesPage : Page
    {
        private readonly TouchService _service = new();
        private List<ColumnSetting> _columnSettings = new();
        private List<TouchViewModel> _allData = new();
        private List<TouchViewModel> _currentPageData = new();
        private int _currentPage = 1;
        private int _pageSize = 25;

        public TouchesPage()
        {
            InitializeComponent();
            InitializeColumnSettings();
        }

        private void InitializeColumnSettings()
        {
            var saved = SettingsService.LoadColumnSettings("Touches");
            if (saved != null && saved.Count == 13)
            {
                _columnSettings = saved;
                return;
            }

            _columnSettings = new List<ColumnSetting>
            {
                new ColumnSetting { Header = "ID" },
                new ColumnSetting { Header = "Название касания" },
                new ColumnSetting { Header = "Тип касания" },
                new ColumnSetting { Header = "Дата создания" },
                new ColumnSetting { Header = "Дата начала" },
                new ColumnSetting { Header = "Дата завершения" },
                new ColumnSetting { Header = "Оператор" },
                new ColumnSetting { Header = "Ячейка" },
                new ColumnSetting { Header = "Результат" },
                new ColumnSetting { Header = "Фотография" },
                new ColumnSetting { Header = "Публикация" },
                new ColumnSetting { Header = "Охват" },
                new ColumnSetting { Header = "Принято куратором" }
            };
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            LoadFilters();
            ApplyColumnVisibility();
        }

        private void LoadData()
        {
            _allData = _service.GetAll(chkCompleted.IsChecked == true)
                .Select(t => new TouchViewModel(t))
                .ToList();
            _currentPage = 1;
            ApplyPagination();
        }

        private void LoadFilters()
        {
            cmbType.ItemsSource = _service.GetTypes();
            cmbType.DisplayMemberPath = "Name";
            cmbType.SelectedValuePath = "Id";

            cmbCell.ItemsSource = new CellService().GetAll();
            cmbCell.DisplayMemberPath = "Name";
            cmbCell.SelectedValuePath = "Id";

            cmbOperator.ItemsSource = new UserService().GetAll();
            cmbOperator.DisplayMemberPath = "FullName";
            cmbOperator.SelectedValuePath = "Id";
        }

        private void ChkCompleted_Checked(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var data = _service.GetAll(chkCompleted.IsChecked == true)
                .Select(t => new TouchViewModel(t))
                .ToList();

            if (cmbType.SelectedValue is int typeId)
                data = data.Where(t => t.TypeName == _service.GetTypes().FirstOrDefault(x => x.Id == typeId)?.Name).ToList();

            if (cmbCell.SelectedValue is int cellId)
                data = data.Where(t => t.CellName == new CellService().GetAll().FirstOrDefault(x => x.Id == cellId)?.Name).ToList();

            if (cmbOperator.SelectedValue is int operatorId)
                data = data.Where(t => t.OperatorName == new UserService().GetAll().FirstOrDefault(x => x.Id == operatorId)?.FullName).ToList();

            _allData = data;
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

            dgTouches.ItemsSource = _currentPageData;
            int from = (_currentPage - 1) * _pageSize + 1;
            int to = System.Math.Min(_currentPage * _pageSize, total);
            txtStatus?.SetCurrentValue(TextBlock.TextProperty, $"Показаны строки с {from} по {to} из {total}");
            txtPageInfo?.SetCurrentValue(TextBlock.TextProperty, $"{_currentPage} / {totalPages}");
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = new TouchEditWindow();
            if (window.ShowDialog() == true)
                LoadData();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgTouches.SelectedItem is not TouchViewModel vm)
                return;

            var touch = _service.GetById(vm.Id);
            if (touch == null)
                return;

            var window = new TouchEditWindow(touch);
            if (window.ShowDialog() == true)
                LoadData();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgTouches.SelectedItem is not TouchViewModel vm)
                return;

            if (MessageBox.Show("Удалить выбранное касание?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            _service.Delete(vm.Id);
            LoadData();
        }

        private void BtnColumns_Click(object sender, RoutedEventArgs e)
        {
            var window = new ColumnSettingsWindow(_columnSettings);
            if (window.ShowDialog() == true)
            {
                _columnSettings = window.Settings;
                SettingsService.SaveColumnSettings("Touches", _columnSettings);
                ApplyColumnVisibility();
            }
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
            var columns = new System.Collections.Generic.Dictionary<string, DataGridColumn>
            {
                ["ID"] = colId,
                ["Название касания"] = colName,
                ["Тип касания"] = colType,
                ["Дата создания"] = colCreatedAt,
                ["Дата начала"] = colStartDate,
                ["Дата завершения"] = colEndDate,
                ["Оператор"] = colOperator,
                ["Ячейка"] = colCell,
                ["Результат"] = colResult,
                ["Фотография"] = colPhoto,
                ["Публикация"] = colPublication,
                ["Охват"] = colCoverage,
                ["Принято куратором"] = colApproved
            };

            foreach (var setting in _columnSettings)
            {
                if (columns.TryGetValue(setting.Header, out var column))
                    column.Visibility = setting.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }

    public class TouchViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string TypeName { get; set; } = string.Empty;
        public string CreatedAtText { get; set; } = string.Empty;
        public string StartDateText { get; set; } = string.Empty;
        public string EndDateText { get; set; } = string.Empty;
        public string OperatorName { get; set; } = string.Empty;
        public string CellName { get; set; } = string.Empty;
        public string ResultText { get; set; } = string.Empty;
        public bool HasPhoto { get; set; }
        public bool HasPublication { get; set; }
        public int? Coverage { get; set; }
        public bool IsCuratorApproved { get; set; }

        public TouchViewModel(Touch touch)
        {
            Id = touch.Id;
            Name = touch.Name;
            TypeName = touch.Type?.Name ?? string.Empty;
            CreatedAtText = touch.CreatedAt.ToString("dd.MM.yyyy");
            StartDateText = touch.StartDate?.ToString("dd.MM.yyyy") ?? string.Empty;
            EndDateText = touch.EndDate?.ToString("dd.MM.yyyy") ?? string.Empty;
            OperatorName = touch.Operator?.FullName ?? string.Empty;
            CellName = touch.Cell?.Name ?? string.Empty;
            ResultText = touch.ResultText;
            HasPhoto = touch.HasPhoto;
            HasPublication = touch.HasPublication;
            Coverage = touch.Coverage;
            IsCuratorApproved = touch.IsCuratorApproved;
        }
    }
}
