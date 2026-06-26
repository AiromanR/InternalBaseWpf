using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InternalBaseWpf.Models;
using InternalBaseWpf.Service;

namespace InternalBaseWpf.Pages
{
    public partial class DuplicatesPage : Page
    {
        private readonly ActivistService _service = new();
        private List<DuplicateViewModel> _allData = new();
        private List<DuplicateViewModel> _currentPageData = new();
        private int _currentPage = 1;
        private int _pageSize = 25;

        public DuplicatesPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            _allData = _service.GetDuplicates().Select(a => new DuplicateViewModel(a)).ToList();
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

            dgDuplicates.ItemsSource = _currentPageData;
            int from = (_currentPage - 1) * _pageSize + 1;
            int to = System.Math.Min(_currentPage * _pageSize, total);
            txtStatus?.SetCurrentValue(TextBlock.TextProperty, $"Показаны строки с {from} по {to} из {total}");
            txtPageInfo?.SetCurrentValue(TextBlock.TextProperty, $"{_currentPage} / {totalPages}");
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgDuplicates.SelectedItem is not DuplicateViewModel vm)
                return;

            if (MessageBox.Show($"Удалить запись \"{vm.FullName}\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            _service.Delete(vm.Id);
            LoadData();
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
    }

    public class DuplicateViewModel
    {
        public int Id { get; set; }
        public string DuplicateKey { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? LocalBranch { get; set; }
        public string BirthDateText { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string CreatedAtText { get; set; } = string.Empty;

        public DuplicateViewModel(Activist activist)
        {
            Id = activist.Id;
            FullName = activist.FullName;
            LocalBranch = activist.LocalBranch;
            BirthDateText = activist.BirthDate?.ToString("dd.MM.yyyy") ?? string.Empty;
            Phone = activist.Phone;
            Address = activist.Address;
            CreatedAtText = activist.CreatedAt.ToString("dd.MM.yyyy HH:mm");
            DuplicateKey = $"{FullName} | {BirthDateText}";
        }
    }
}
