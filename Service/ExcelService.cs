using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InternalBaseWpf.Models;
using OfficeOpenXml;

namespace InternalBaseWpf.Service
{
    public static class ExcelService
    {
        private static readonly string[] ImportHeaders = new[]
        {
            "Фамилия", "Имя", "Отчество", "Пол", "Дата рождения", "Моб. телефон", "E-mail", "Адрес",
            "Местное отделение", "Первичное отделение", "УИК №", "Основная ячейка", "Ячейки",
            "Место работы", "Должность", "Примечание"
        };

        static ExcelService()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public static void CreateTemplate(string filePath)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Шаблон");
            for (int i = 0; i < ImportHeaders.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = ImportHeaders[i];
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
            }
            worksheet.Cells.AutoFitColumns();
            package.SaveAs(new FileInfo(filePath));
        }

        public static List<Activist> ImportFromExcel(string filePath, out List<string> errors)
        {
            errors = new List<string>();
            var result = new List<Activist>();
            var cellService = new CellService();
            var cells = cellService.GetAll();

            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0];
            int rowCount = worksheet.Dimension?.Rows ?? 0;

            for (int row = 2; row <= rowCount; row++)
            {
                string lastName = worksheet.Cells[row, 1].Text.Trim();
                string firstName = worksheet.Cells[row, 2].Text.Trim();
                if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName))
                {
                    errors.Add($"Строка {row}: не указана фамилия или имя");
                    continue;
                }

                var activist = new Activist
                {
                    LastName = lastName,
                    FirstName = firstName,
                    Patronymic = worksheet.Cells[row, 3].Text.Trim(),
                    Gender = worksheet.Cells[row, 4].Text.Trim(),
                    BirthDate = ParseDate(worksheet.Cells[row, 5].Text),
                    Phone = worksheet.Cells[row, 6].Text.Trim(),
                    Email = worksheet.Cells[row, 7].Text.Trim(),
                    Address = worksheet.Cells[row, 8].Text.Trim(),
                    LocalBranch = worksheet.Cells[row, 9].Text.Trim(),
                    PrimaryBranch = worksheet.Cells[row, 10].Text.Trim(),
                    UikNumber = worksheet.Cells[row, 11].Text.Trim(),
                    Workplace = worksheet.Cells[row, 14].Text.Trim(),
                    Position = worksheet.Cells[row, 15].Text.Trim(),
                    Note = worksheet.Cells[row, 16].Text.Trim(),
                    IsConfirmed = false
                };

                string mainCellName = worksheet.Cells[row, 12].Text.Trim();
                if (!string.IsNullOrWhiteSpace(mainCellName))
                {
                    var cell = cells.FirstOrDefault(c => c.Name == mainCellName);
                    if (cell != null)
                        activist.MainCellId = cell.Id;
                }

                string cellNames = worksheet.Cells[row, 13].Text.Trim();
                if (!string.IsNullOrWhiteSpace(cellNames))
                {
                    foreach (var name in cellNames.Split(','))
                    {
                        var cell = cells.FirstOrDefault(c => c.Name == name.Trim());
                        if (cell != null)
                            activist.ActivistCells.Add(new ActivistCell { CellId = cell.Id });
                    }
                }

                result.Add(activist);
            }

            return result;
        }

        public static void ExportActivists(string filePath, List<Activist> activists)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Активисты");
            string[] headers = { "ФИО", "Дата рождения", "Моб. телефон", "Адрес", "Местное отделение", "Основная ячейка", "Дата создания" };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
            }

            for (int i = 0; i < activists.Count; i++)
            {
                var a = activists[i];
                worksheet.Cells[i + 2, 1].Value = a.FullName;
                worksheet.Cells[i + 2, 2].Value = a.BirthDate?.ToString("dd.MM.yyyy");
                worksheet.Cells[i + 2, 3].Value = a.Phone;
                worksheet.Cells[i + 2, 4].Value = a.Address;
                worksheet.Cells[i + 2, 5].Value = a.LocalBranch;
                worksheet.Cells[i + 2, 6].Value = a.MainCell?.Name;
                worksheet.Cells[i + 2, 7].Value = a.CreatedAt.ToString("dd.MM.yyyy HH:mm");
            }

            worksheet.Cells.AutoFitColumns();
            package.SaveAs(new FileInfo(filePath));
        }

        private static DateTime? ParseDate(string text)
        {
            if (DateTime.TryParse(text, out DateTime date))
                return date;
            return null;
        }
    }
}
