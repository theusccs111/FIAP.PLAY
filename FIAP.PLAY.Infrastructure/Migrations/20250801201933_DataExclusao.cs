using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIAP.PLAY.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class DataExclusao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataExclusao",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExclusao",
                table: "Jogo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DataExclusao",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataExclusao",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DataExclusao",
                table: "Jogo");
        }
    }
}
