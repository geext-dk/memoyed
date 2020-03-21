using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Memoyed.Cards.ApplicationServices.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardBoxSets",
                columns: table => new
                {
                    DbId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Id = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NativeLanguage = table.Column<string>(nullable: true),
                    TargetLanguage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardBoxSets", x => x.DbId);
                });

            migrationBuilder.CreateTable(
                name: "RevisionSessions",
                columns: table => new
                {
                    DbId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevisionSessions", x => x.DbId);
                });

            migrationBuilder.CreateTable(
                name: "CardBoxes",
                columns: table => new
                {
                    DbId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Id = table.Column<Guid>(nullable: true),
                    SetId = table.Column<Guid>(nullable: true),
                    Level = table.Column<int>(nullable: true),
                    RevisionDelay = table.Column<int>(nullable: true),
                    CardBoxSetDbId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardBoxes", x => x.DbId);
                    table.ForeignKey(
                        name: "FK_CardBoxes_CardBoxSets_CardBoxSetDbId",
                        column: x => x.CardBoxSetDbId,
                        principalTable: "CardBoxSets",
                        principalColumn: "DbId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CardBoxSets_CompletedRevisionSessionIds",
                columns: table => new
                {
                    CardBoxSetDbId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardBoxSets_CompletedRevisionSessionIds", x => new { x.CardBoxSetDbId, x.Id });
                    table.ForeignKey(
                        name: "FK_CardBoxSets_CompletedRevisionSessionIds_CardBoxSets_CardBoxSetDbId",
                        column: x => x.CardBoxSetDbId,
                        principalTable: "CardBoxSets",
                        principalColumn: "DbId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionCards",
                columns: table => new
                {
                    DbId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<Guid>(nullable: true),
                    CardId = table.Column<Guid>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    NativeLanguageWord = table.Column<string>(nullable: true),
                    TargetLanguageWord = table.Column<string>(nullable: true),
                    RevisionSessionDbId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionCards", x => x.DbId);
                    table.ForeignKey(
                        name: "FK_SessionCards_RevisionSessions_RevisionSessionDbId",
                        column: x => x.RevisionSessionDbId,
                        principalTable: "RevisionSessions",
                        principalColumn: "DbId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Domain.Cards",
                columns: table => new
                {
                    DbId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Id = table.Column<Guid>(nullable: true),
                    CardBoxId = table.Column<Guid>(nullable: true),
                    NativeLanguageWord = table.Column<string>(nullable: true),
                    TargetLanguageWord = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    CardBoxChangeDate = table.Column<DateTime>(nullable: true),
                    CardBoxDbId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.DbId);
                    table.ForeignKey(
                        name: "FK_Cards_CardBoxes_CardBoxDbId",
                        column: x => x.CardBoxDbId,
                        principalTable: "CardBoxes",
                        principalColumn: "DbId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardBoxes_CardBoxSetDbId",
                table: "CardBoxes",
                column: "CardBoxSetDbId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardBoxDbId",
                table: "Domain.Cards",
                column: "CardBoxDbId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionCards_RevisionSessionDbId",
                table: "SessionCards",
                column: "RevisionSessionDbId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardBoxSets_CompletedRevisionSessionIds");

            migrationBuilder.DropTable(
                name: "Domain.Cards");

            migrationBuilder.DropTable(
                name: "SessionCards");

            migrationBuilder.DropTable(
                name: "CardBoxes");

            migrationBuilder.DropTable(
                name: "RevisionSessions");

            migrationBuilder.DropTable(
                name: "CardBoxSets");
        }
    }
}
