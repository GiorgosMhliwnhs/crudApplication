using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace peripatoiCrud.API.Migrations
{
    /// <inheritdoc />
    public partial class ArxikoMigrationInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dyskolies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Onoma = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dyskolies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Perioxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kwdikos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Onoma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EikonaUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perioxes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Peripatoi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Onoma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Perigrafh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mhkos = table.Column<double>(type: "float", nullable: false),
                    EikonaUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DyskoliaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PerioxhId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peripatoi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Peripatoi_Dyskolies_DyskoliaId",
                        column: x => x.DyskoliaId,
                        principalTable: "Dyskolies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Peripatoi_Perioxes_PerioxhId",
                        column: x => x.PerioxhId,
                        principalTable: "Perioxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Peripatoi_DyskoliaId",
                table: "Peripatoi",
                column: "DyskoliaId");

            migrationBuilder.CreateIndex(
                name: "IX_Peripatoi_PerioxhId",
                table: "Peripatoi",
                column: "PerioxhId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Peripatoi");

            migrationBuilder.DropTable(
                name: "Dyskolies");

            migrationBuilder.DropTable(
                name: "Perioxes");
        }
    }
}
