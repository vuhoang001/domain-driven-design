using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Item.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInternalCommand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OccurredOn",
                table: "InternalCommands",
                newName: "EnqueueDate");

            migrationBuilder.AddColumn<string>(
                name: "Error",
                table: "InternalCommands",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Error",
                table: "InternalCommands");

            migrationBuilder.RenameColumn(
                name: "EnqueueDate",
                table: "InternalCommands",
                newName: "OccurredOn");
        }
    }
}
