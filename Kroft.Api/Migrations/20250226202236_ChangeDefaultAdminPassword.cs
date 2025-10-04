using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kroft.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDefaultAdminPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserProfiles",
                keyColumn: "ID",
                keyValue: new Guid("61889689-5a72-40ec-bf80-560b85d5775b"),
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "JGSbvfD04Sc5pxiNgfb4ow==", "bJZ9DdwaYkrQQ8/S+Opj/A==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserProfiles",
                keyColumn: "ID",
                keyValue: new Guid("61889689-5a72-40ec-bf80-560b85d5775b"),
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "RTpOp2VygaXUlR+tllsNSw==", "ARDtz6IUJ6q9NHY26t6FTg==" });
        }
    }
}
