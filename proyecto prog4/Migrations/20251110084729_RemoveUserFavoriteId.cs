using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proyecto_prog4.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserFavoriteId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserFavorites");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserFavorites",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
