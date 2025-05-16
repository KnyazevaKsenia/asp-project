using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspProject.Migrations
{
    /// <inheritdoc />
    public partial class AddedDurationToTicketSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Course",
                table: "TicketsSets");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TicketsSets",
                newName: "StudentId");
        }
        
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "TicketsSets",
                newName: "UserId");

            migrationBuilder.AddColumn<int>(
                name: "Course",
                table: "TicketsSets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
