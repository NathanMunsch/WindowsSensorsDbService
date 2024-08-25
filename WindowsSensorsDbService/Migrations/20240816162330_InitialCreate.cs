using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WindowsSensorsDbService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComputerEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HardwareEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HardwareEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DateMeasurementEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComputerEntityId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateMeasurementEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DateMeasurementEntities_ComputerEntities_ComputerEntityId",
                        column: x => x.ComputerEntityId,
                        principalTable: "ComputerEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateMeasurementEntityId = table.Column<int>(type: "int", nullable: false),
                    HardwareEntityId = table.Column<int>(type: "int", nullable: false),
                    SensorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeasuredValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeasurementEntities_DateMeasurementEntities_DateMeasurementEntityId",
                        column: x => x.DateMeasurementEntityId,
                        principalTable: "DateMeasurementEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeasurementEntities_HardwareEntities_HardwareEntityId",
                        column: x => x.HardwareEntityId,
                        principalTable: "HardwareEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DateMeasurementEntities_ComputerEntityId",
                table: "DateMeasurementEntities",
                column: "ComputerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementEntities_DateMeasurementEntityId",
                table: "MeasurementEntities",
                column: "DateMeasurementEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementEntities_HardwareEntityId",
                table: "MeasurementEntities",
                column: "HardwareEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeasurementEntities");

            migrationBuilder.DropTable(
                name: "DateMeasurementEntities");

            migrationBuilder.DropTable(
                name: "HardwareEntities");

            migrationBuilder.DropTable(
                name: "ComputerEntities");
        }
    }
}
