using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIAP.PLAY.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fix_config : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameLibrary_Game_GameId",
                table: "GameLibrary");

            migrationBuilder.AddForeignKey(
                name: "FK_GameLibrary_Game_GameId",
                table: "GameLibrary",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameLibrary_Game_GameId",
                table: "GameLibrary");

            migrationBuilder.AddForeignKey(
                name: "FK_GameLibrary_Game_GameId",
                table: "GameLibrary",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
