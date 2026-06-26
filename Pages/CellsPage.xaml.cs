using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InternalBaseWpf.Models;
using InternalBaseWpf.Service;
using InternalBaseWpf.Windows;

namespace InternalBaseWpf.Pages
{
    public partial class CellsPage : Page
    {
        private readonly CellService _service = new();

        public CellsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            var cells = _service.GetAll();
            lbCells.ItemsSource = cells;
        }


        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = txtSearch.Text.Trim();
            lbCells.ItemsSource = string.IsNullOrEmpty(query)
                ? _service.GetAll()
                : _service.Search(query);
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            LoadData();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog("Введите название ячейки:", "Добавление ячейки");
            if (dialog.ShowDialog() != true)
                return;

            string name = dialog.ResponseText;
            if (string.IsNullOrWhiteSpace(name))
                return;

            _service.Add(new Cell { Name = name });
            LoadData();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lbCells.SelectedItem is not Cell cell)
                return;

            var dialog = new InputDialog("Введите новое название ячейки:", "Изменение ячейки", cell.Name);
            if (dialog.ShowDialog() != true)
                return;

            string name = dialog.ResponseText;
            if (string.IsNullOrWhiteSpace(name))
                return;

            cell.Name = name;
            _service.Update(cell);
            LoadData();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lbCells.SelectedItem is not Cell cell)
                return;

            if (MessageBox.Show($"Удалить ячейку \"{cell.Name}\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            _service.Delete(cell.Id);
            LoadData();
        }

        private void CmbPageSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void BtnPage_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
