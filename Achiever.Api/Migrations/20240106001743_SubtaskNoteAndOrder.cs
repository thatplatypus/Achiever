using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Achiever.Api.Migrations
{
    /// <inheritdoc />
    public partial class SubtaskNoteAndOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "SubTasks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "SubTasks",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "SubTasks");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "SubTasks");
        }
    }
}
