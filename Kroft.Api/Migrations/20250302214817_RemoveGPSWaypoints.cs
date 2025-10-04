using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kroft.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGPSWaypoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Waypoints");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Waypoints",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Altitude = table.Column<double>(type: "REAL", nullable: true),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    SearchRadius = table.Column<double>(type: "REAL", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: true),
                    WaypointColor = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Waypoints", x => x.ID);
                });
        }
    }
}
