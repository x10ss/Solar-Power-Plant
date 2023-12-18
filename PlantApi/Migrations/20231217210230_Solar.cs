using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantApi.Migrations
{
    /// <inheritdoc />
    public partial class Solar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SolarPowerPlant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlantInstalledPower = table.Column<float>(type: "real", nullable: false),
                    DateInstalled = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false),
                    Latitude = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolarPowerPlant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolarPowerPlantUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolarPowerPlantUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolarPowerPlantData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GranulomCount = table.Column<int>(type: "int", nullable: false),
                    ActualPower = table.Column<float>(type: "real", nullable: false),
                    ForecastedPower = table.Column<float>(type: "real", nullable: false),
                    SolarPowerPlantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolarPowerPlantData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolarPowerPlantData_SolarPowerPlant_SolarPowerPlantId",
                        column: x => x.SolarPowerPlantId,
                        principalTable: "SolarPowerPlant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolarPowerPlantData_GranulomCount_SolarPowerPlantId",
                table: "SolarPowerPlantData",
                columns: new[] { "GranulomCount", "SolarPowerPlantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolarPowerPlantData_SolarPowerPlantId",
                table: "SolarPowerPlantData",
                column: "SolarPowerPlantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolarPowerPlantData");

            migrationBuilder.DropTable(
                name: "SolarPowerPlantUser");

            migrationBuilder.DropTable(
                name: "SolarPowerPlant");
        }
    }
}
