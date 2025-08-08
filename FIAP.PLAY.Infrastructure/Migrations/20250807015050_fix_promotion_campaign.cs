using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIAP.PLAY.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fix_promotion_campaign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Campaign_CampaignId",
                table: "Promotions");

            migrationBuilder.DropTable(
                name: "PromotionGames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Promotions",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Promotions");

            migrationBuilder.RenameTable(
                name: "Promotions",
                newName: "Promotion");

            migrationBuilder.RenameIndex(
                name: "IX_Promotions_CampaignId",
                table: "Promotion",
                newName: "IX_Promotion_CampaignId");

            migrationBuilder.AddColumn<long>(
                name: "PromotionId",
                table: "Game",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Campaign",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPercentage",
                table: "Promotion",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Promotion",
                table: "Promotion",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PromotionGame",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionId = table.Column<long>(type: "bigint", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionGame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionGame_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionGame_Promotion_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_PromotionId",
                table: "Game",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionGame_GameId",
                table: "PromotionGame",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionGame_PromotionId",
                table: "PromotionGame",
                column: "PromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Promotion_PromotionId",
                table: "Game",
                column: "PromotionId",
                principalTable: "Promotion",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotion_Campaign_CampaignId",
                table: "Promotion",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Promotion_PromotionId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Promotion_Campaign_CampaignId",
                table: "Promotion");

            migrationBuilder.DropTable(
                name: "PromotionGame");

            migrationBuilder.DropIndex(
                name: "IX_Game_PromotionId",
                table: "Game");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Promotion",
                table: "Promotion");

            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "Game");

            migrationBuilder.RenameTable(
                name: "Promotion",
                newName: "Promotions");

            migrationBuilder.RenameIndex(
                name: "IX_Promotion_CampaignId",
                table: "Promotions",
                newName: "IX_Promotions_CampaignId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Campaign",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Campaign",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPercentage",
                table: "Promotions",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Promotions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Promotions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Promotions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Promotions",
                table: "Promotions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PromotionGames",
                columns: table => new
                {
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    PromotionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionGames", x => new { x.GameId, x.PromotionId });
                    table.ForeignKey(
                        name: "FK_PromotionGames_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionGames_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionGames_PromotionId",
                table: "PromotionGames",
                column: "PromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Campaign_CampaignId",
                table: "Promotions",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
