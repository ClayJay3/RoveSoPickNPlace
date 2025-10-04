using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kroft.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialDBCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    HashedPassword = table.Column<string>(type: "TEXT", nullable: true),
                    Salt = table.Column<string>(type: "TEXT", nullable: true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    State = table.Column<string>(type: "TEXT", nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: true),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PermissionLevel = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Waypoints",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    Altitude = table.Column<double>(type: "REAL", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: true),
                    WaypointColor = table.Column<int>(type: "INTEGER", nullable: true),
                    SearchRadius = table.Column<double>(type: "REAL", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Waypoints", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "ID", "Address", "City", "Country", "CreatedAt", "DateOfBirth", "Email", "FirstName", "HashedPassword", "LastName", "PermissionLevel", "PhoneNumber", "PostalCode", "Salt", "State", "UpdatedAt", "Username" },
                values: new object[] { new Guid("61889689-5a72-40ec-bf80-560b85d5775b"), "123 Admin St", "Admin City", "Admin Country", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", "Admin", "A93ehbodEKbfuFxWmcKaB0SoPb47/twlnlnXt2bJYJS3ofrr8WTmOnt6oWKhpzpzbuXxdXUdF5ScSF5PBHDhkg==", "User", 3, "123-456-7890", "12345", "nNXwKB8R6/KR2M3aWjFX/PyqSQ0WfNAL7qNARdyFdMujoGC8H2c7JcumlQckbvIG0sWyrzeKKAOHCXbaVnov23aAzrIOEGttZkZRYVspzGCoOMyOxwm99SyAWp2eWlSn2F977hAs0ic3JQ0cKCcCHQKYNUbZmXWikeXXBqKLnLM=", "Admin State", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Waypoints");
        }
    }
}
