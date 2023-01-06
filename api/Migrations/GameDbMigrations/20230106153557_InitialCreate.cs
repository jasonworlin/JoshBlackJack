using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations.GameDbMigrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    DeckId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.DeckId);
                });

            migrationBuilder.CreateTable(
                name: "Hands",
                columns: table => new
                {
                    HandId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hands", x => x.HandId);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<int>(type: "INTEGER", nullable: false),
                    Suit = table.Column<string>(type: "TEXT", nullable: false),
                    DeckId = table.Column<int>(type: "INTEGER", nullable: true),
                    HandId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                    table.ForeignKey(
                        name: "FK_Cards_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "DeckId");
                    table.ForeignKey(
                        name: "FK_Cards_Hands_HandId",
                        column: x => x.HandId,
                        principalTable: "Hands",
                        principalColumn: "HandId");
                });

            migrationBuilder.CreateTable(
                name: "Dealers",
                columns: table => new
                {
                    DealerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Total = table.Column<int>(type: "INTEGER", nullable: false),
                    HandId = table.Column<int>(type: "INTEGER", nullable: false),
                    HasBusted = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasStuck = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasSplit = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dealers", x => x.DealerId);
                    table.ForeignKey(
                        name: "FK_Dealers_Hands_HandId",
                        column: x => x.HandId,
                        principalTable: "Hands",
                        principalColumn: "HandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Balance = table.Column<decimal>(type: "TEXT", nullable: false),
                    Hand1HandId = table.Column<int>(type: "INTEGER", nullable: false),
                    Hand2HandId = table.Column<int>(type: "INTEGER", nullable: false),
                    HasBusted = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasStuck = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanSplit = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasWon = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Players_Hands_Hand1HandId",
                        column: x => x.Hand1HandId,
                        principalTable: "Hands",
                        principalColumn: "HandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Players_Hands_Hand2HandId",
                        column: x => x.Hand2HandId,
                        principalTable: "Hands",
                        principalColumn: "HandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeckId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    DealerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_Games_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "DealerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "DeckId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bots",
                columns: table => new
                {
                    BotId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Hand1HandId = table.Column<int>(type: "INTEGER", nullable: false),
                    Hand2HandId = table.Column<int>(type: "INTEGER", nullable: false),
                    HasBusted = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasStuck = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasSplit = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasWon = table.Column<bool>(type: "INTEGER", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bots", x => x.BotId);
                    table.ForeignKey(
                        name: "FK_Bots_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId");
                    table.ForeignKey(
                        name: "FK_Bots_Hands_Hand1HandId",
                        column: x => x.Hand1HandId,
                        principalTable: "Hands",
                        principalColumn: "HandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bots_Hands_Hand2HandId",
                        column: x => x.Hand2HandId,
                        principalTable: "Hands",
                        principalColumn: "HandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bots_GameId",
                table: "Bots",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Bots_Hand1HandId",
                table: "Bots",
                column: "Hand1HandId");

            migrationBuilder.CreateIndex(
                name: "IX_Bots_Hand2HandId",
                table: "Bots",
                column: "Hand2HandId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_DeckId",
                table: "Cards",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_HandId",
                table: "Cards",
                column: "HandId");

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_HandId",
                table: "Dealers",
                column: "HandId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_DealerId",
                table: "Games",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_DeckId",
                table: "Games",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_PlayerId",
                table: "Games",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Hand1HandId",
                table: "Players",
                column: "Hand1HandId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Hand2HandId",
                table: "Players",
                column: "Hand2HandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bots");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Dealers");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Hands");
        }
    }
}
