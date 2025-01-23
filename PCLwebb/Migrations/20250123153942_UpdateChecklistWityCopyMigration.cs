using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCLwebb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChecklistWityCopyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTemplate",
                table: "Checklists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ParentChecklistID",
                table: "Checklists",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Checklists_ParentChecklistID",
                table: "Checklists",
                column: "ParentChecklistID");

            migrationBuilder.AddForeignKey(
                name: "FK_Checklists_Checklists_ParentChecklistID",
                table: "Checklists",
                column: "ParentChecklistID",
                principalTable: "Checklists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklists_Checklists_ParentChecklistID",
                table: "Checklists");

            migrationBuilder.DropIndex(
                name: "IX_Checklists_ParentChecklistID",
                table: "Checklists");

            migrationBuilder.DropColumn(
                name: "IsTemplate",
                table: "Checklists");

            migrationBuilder.DropColumn(
                name: "ParentChecklistID",
                table: "Checklists");
        }
    }
}
