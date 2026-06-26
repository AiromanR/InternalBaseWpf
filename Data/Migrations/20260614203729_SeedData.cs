using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InternalBaseWpf.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Activists",
                columns: new[] { "Id", "Address", "BirthDate", "CanEdit", "CitizenshipCategory", "CreatedAt", "DeletionReason", "Email", "FirstName", "Gender", "ImportStatus", "IsAts", "IsConfirmed", "IsPartySupporter", "IsPresidentSupporter", "IsResponsible", "IsVdlSupporter", "IsVverh", "LastName", "LocalBranch", "Loyalty", "MainCellId", "MarkedForDeletionByResponsible", "Note", "OrgStatus", "PartyStatus", "Patronymic", "Phone", "Position", "PrimaryBranch", "Readiness", "RegionalReadiness", "ResponsibleUserId", "TargetAudience", "UikNumber", "Workplace" },
                values: new object[,]
                {
                    { 1, "г. Пермь, ул. Ленина, д. 1", new DateTime(1980, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Иван", "Мужской", null, false, true, false, false, false, false, false, "Иванов", "МО Куединского муниципального округа", null, null, false, null, null, null, "Иванович", "(912) 345-67-89", null, null, null, null, null, null, "1822", null },
                    { 2, "г. Пермь, ул. Гагарина, д. 5", new DateTime(1992, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Мария", "Женский", null, false, true, false, false, false, false, false, "Петрова", "МО Куединского муниципального округа", null, 2, false, null, null, null, "Сергеевна", "(922) 456-78-90", null, null, null, null, null, null, "1823", null },
                    { 3, "г. Пермь, ул. Ленина, д. 2", new DateTime(1980, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Иван", "Мужской", null, false, true, false, false, false, false, false, "Иванов", "МО Куединского муниципального округа", null, null, false, null, null, null, "Иванович", "(912) 345-67-99", null, null, null, null, null, null, "1822", null }
                });

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 37, 29, 470, DateTimeKind.Local).AddTicks(2908));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 37, 29, 470, DateTimeKind.Local).AddTicks(2911));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 37, 29, 470, DateTimeKind.Local).AddTicks(2912));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 37, 29, 470, DateTimeKind.Local).AddTicks(2913));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 37, 29, 470, DateTimeKind.Local).AddTicks(2914));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 37, 29, 470, DateTimeKind.Local).AddTicks(2915));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 37, 29, 470, DateTimeKind.Local).AddTicks(2916));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 37, 29, 470, DateTimeKind.Local).AddTicks(2917));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 37, 29, 470, DateTimeKind.Local).AddTicks(2918));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 37, 29, 470, DateTimeKind.Local).AddTicks(2919));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 37, 29, 470, DateTimeKind.Local).AddTicks(2920));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 37, 29, 470, DateTimeKind.Local).AddTicks(2921));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 37, 29, 470, DateTimeKind.Local).AddTicks(2922));

            migrationBuilder.InsertData(
                table: "Touches",
                columns: new[] { "Id", "CellId", "Coverage", "CreatedAt", "Description", "EndDate", "HasPhoto", "HasPublication", "IsActive", "IsCuratorApproved", "IsRikPlaced", "IsShopPlaced", "MediaLink", "Name", "OperatorId", "SocialLink", "StartDate", "TypeId" },
                values: new object[,]
                {
                    { 1, 2, 40, new DateTime(2026, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, true, false, false, false, null, "День России", 2, null, new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 2, 60, new DateTime(2026, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, true, false, false, false, null, "День молодёжи", 2, null, new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "ktOd8bsFgodSF5PAcB+O2X3AI4kwnN7sDJHQ5a0SIzA=", "Mo7vjCmXyZj+O23K8eMD3g==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "ktOd8bsFgodSF5PAcB+O2X3AI4kwnN7sDJHQ5a0SIzA=", "Mo7vjCmXyZj+O23K8eMD3g==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activists",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Activists",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Activists",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Touches",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Touches",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(826));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(830));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(831));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(832));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(833));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(834));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(835));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(836));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(837));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(838));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(839));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(840));

            migrationBuilder.UpdateData(
                table: "Cells",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(841));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "Ds0HkiyNb9LeNStqweOQ/IGzP2c4jJ5TBMIOAnE3PjQ=", "hU4MJ0AqvUKNBV56RJA/WA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "Ds0HkiyNb9LeNStqweOQ/IGzP2c4jJ5TBMIOAnE3PjQ=", "hU4MJ0AqvUKNBV56RJA/WA==" });
        }
    }
}
