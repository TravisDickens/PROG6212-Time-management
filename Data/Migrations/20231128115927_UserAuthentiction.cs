using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeManagementPOE.Data.Migrations
{
    public partial class UserAuthentiction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "StudyHours",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Semesters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StudyHours");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Semesters");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Modules");
        }
    }
}
