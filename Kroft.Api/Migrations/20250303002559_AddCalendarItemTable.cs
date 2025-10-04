using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kroft.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCalendarItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalendarItems",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: true),
                    End = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AllDay = table.Column<bool>(type: "INTEGER", nullable: true),
                    Color = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarItems", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarItems");
        }
    }
}
