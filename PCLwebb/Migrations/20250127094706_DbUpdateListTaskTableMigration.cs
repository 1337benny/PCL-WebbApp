using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCLwebb.Migrations
{
    /// <inheritdoc />
    public partial class DbUpdateListTaskTableMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EditedDate",
                table: "ListTasks",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditedDate",
                table: "ListTasks");
        }
    }
}
