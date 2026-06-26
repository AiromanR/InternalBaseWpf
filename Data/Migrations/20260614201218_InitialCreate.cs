using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InternalBaseWpf.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cells",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cells", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TouchTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouchTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalBranch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryBranch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UikNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsResponsible = table.Column<bool>(type: "bit", nullable: false),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false),
                    ResponsibleUserId = table.Column<int>(type: "int", nullable: true),
                    MainCellId = table.Column<int>(type: "int", nullable: true),
                    PartyStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrgStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Workplace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPartySupporter = table.Column<bool>(type: "bit", nullable: false),
                    IsPresidentSupporter = table.Column<bool>(type: "bit", nullable: false),
                    IsVdlSupporter = table.Column<bool>(type: "bit", nullable: false),
                    Loyalty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetAudience = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CitizenshipCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAts = table.Column<bool>(type: "bit", nullable: false),
                    IsVverh = table.Column<bool>(type: "bit", nullable: false),
                    Readiness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionalReadiness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarkedForDeletionByResponsible = table.Column<bool>(type: "bit", nullable: false),
                    DeletionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    ImportStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activists_Cells_MainCellId",
                        column: x => x.MainCellId,
                        principalTable: "Cells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Activists_Users_ResponsibleUserId",
                        column: x => x.ResponsibleUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Touches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SocialLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Coverage = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    CellId = table.Column<int>(type: "int", nullable: true),
                    OperatorId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsShopPlaced = table.Column<bool>(type: "bit", nullable: false),
                    IsRikPlaced = table.Column<bool>(type: "bit", nullable: false),
                    HasPhoto = table.Column<bool>(type: "bit", nullable: false),
                    HasPublication = table.Column<bool>(type: "bit", nullable: false),
                    IsCuratorApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Touches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Touches_Cells_CellId",
                        column: x => x.CellId,
                        principalTable: "Cells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Touches_TouchTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TouchTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Touches_Users_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ActivistCells",
                columns: table => new
                {
                    ActivistId = table.Column<int>(type: "int", nullable: false),
                    CellId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivistCells", x => new { x.ActivistId, x.CellId });
                    table.ForeignKey(
                        name: "FK_ActivistCells_Activists_ActivistId",
                        column: x => x.ActivistId,
                        principalTable: "Activists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivistCells_Cells_CellId",
                        column: x => x.CellId,
                        principalTable: "Cells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cells",
                columns: new[] { "Id", "CreatedAt", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(826), true, "МО Куединского р-на" },
                    { 2, new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(830), true, "Куединское РДК" },
                    { 3, new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(831), true, "Сайт Конституция МО Куединского муниципального округа" },
                    { 4, new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(832), true, "Сторонники МО Куединского муниципального округа" },
                    { 5, new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(833), true, "Молодёжный совет Куединского МО" },
                    { 6, new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(834), true, "Совет ветеранов Куединского МО" },
                    { 7, new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(835), true, "ТИК Куединского МО" },
                    { 8, new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(836), true, "Общественная палата Куединского МО" },
                    { 9, new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(837), true, "Центральная библиотека Куединского МО" },
                    { 10, new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(838), true, "Краеведческий музей Куединского МО" },
                    { 11, new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(839), true, "Спортивный клуб Куединского МО" },
                    { 12, new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(840), true, "Волонтёрский центр Куединского МО" },
                    { 13, new DateTime(2026, 6, 15, 1, 12, 17, 760, DateTimeKind.Local).AddTicks(841), true, "Куединский районный Дом культуры" }
                });

            migrationBuilder.InsertData(
                table: "TouchTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Оффлайн мероприятие" },
                    { 2, "Онлайн мероприятие" },
                    { 3, "Иное" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "FullName", "IsActive", "IsAdmin", "Login", "PasswordHash", "Salt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Администратор", true, true, "admin", "Ds0HkiyNb9LeNStqweOQ/IGzP2c4jJ5TBMIOAnE3PjQ=", "hU4MJ0AqvUKNBV56RJA/WA==" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ирина Рогова", true, false, "rogova", "Ds0HkiyNb9LeNStqweOQ/IGzP2c4jJ5TBMIOAnE3PjQ=", "hU4MJ0AqvUKNBV56RJA/WA==" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivistCells_CellId",
                table: "ActivistCells",
                column: "CellId");

            migrationBuilder.CreateIndex(
                name: "IX_Activists_MainCellId",
                table: "Activists",
                column: "MainCellId");

            migrationBuilder.CreateIndex(
                name: "IX_Activists_ResponsibleUserId",
                table: "Activists",
                column: "ResponsibleUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Touches_CellId",
                table: "Touches",
                column: "CellId");

            migrationBuilder.CreateIndex(
                name: "IX_Touches_OperatorId",
                table: "Touches",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Touches_TypeId",
                table: "Touches",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivistCells");

            migrationBuilder.DropTable(
                name: "Touches");

            migrationBuilder.DropTable(
                name: "Activists");

            migrationBuilder.DropTable(
                name: "TouchTypes");

            migrationBuilder.DropTable(
                name: "Cells");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
