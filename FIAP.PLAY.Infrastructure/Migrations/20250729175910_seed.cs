using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIAP.PLAY.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Ativo", "DataAlteracao", "DataCriacao", "Email", "Nome", "Perfil", "Senha" },
                values: new object[] { 1L, true, new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", "admin", 0, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L);
        }
    }
}
