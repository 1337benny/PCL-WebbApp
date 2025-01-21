using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCLwebb.Migrations
{
    /// <inheritdoc />
    public partial class UserChecklistBindingMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorID",
                table: "Checklists",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Checklists_CreatorID",
                table: "Checklists",
                column: "CreatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Checklists_AspNetUsers_CreatorID",
                table: "Checklists",
                column: "CreatorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklists_AspNetUsers_CreatorID",
                table: "Checklists");

            migrationBuilder.DropIndex(
                name: "IX_Checklists_CreatorID",
                table: "Checklists");

            migrationBuilder.DropColumn(
                name: "CreatorID",
                table: "Checklists");
        }
    }
}
