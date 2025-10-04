using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kroft.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToArgon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Argon2DegreeOfParallelism",
                table: "UserProfiles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Argon2Iterations",
                table: "UserProfiles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Argon2MemorySize",
                table: "UserProfiles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "UserProfiles",
                keyColumn: "ID",
                keyValue: new Guid("61889689-5a72-40ec-bf80-560b85d5775b"),
                columns: new[] { "Argon2DegreeOfParallelism", "Argon2Iterations", "Argon2MemorySize", "HashedPassword", "Salt" },
                values: new object[] { 4, 4, 65536, "RTpOp2VygaXUlR+tllsNSw==", "ARDtz6IUJ6q9NHY26t6FTg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Argon2DegreeOfParallelism",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Argon2Iterations",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Argon2MemorySize",
                table: "UserProfiles");

            migrationBuilder.UpdateData(
                table: "UserProfiles",
                keyColumn: "ID",
                keyValue: new Guid("61889689-5a72-40ec-bf80-560b85d5775b"),
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "A93ehbodEKbfuFxWmcKaB0SoPb47/twlnlnXt2bJYJS3ofrr8WTmOnt6oWKhpzpzbuXxdXUdF5ScSF5PBHDhkg==", "nNXwKB8R6/KR2M3aWjFX/PyqSQ0WfNAL7qNARdyFdMujoGC8H2c7JcumlQckbvIG0sWyrzeKKAOHCXbaVnov23aAzrIOEGttZkZRYVspzGCoOMyOxwm99SyAWp2eWlSn2F977hAs0ic3JQ0cKCcCHQKYNUbZmXWikeXXBqKLnLM=" });
        }
    }
}
