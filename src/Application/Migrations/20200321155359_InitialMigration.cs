using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Memoyed.Application.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "CardBoxSets",
                table => new
                {
                    DbId = table.Column<int>()
                        .Annotation("Sqlite:Autoincrement", true),
                    Id = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NativeLanguage = table.Column<string>(nullable: true),
                    TargetLanguage = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_CardBoxSets", x => x.DbId); });

            migrationBuilder.CreateTable(
                "RevisionSessions",
                table => new
                {
                    DbId = table.Column<int>()
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<int>()
                },
                constraints: table => { table.PrimaryKey("PK_RevisionSessions", x => x.DbId); });

            migrationBuilder.CreateTable(
                "CardBoxes",
                table => new
                {
                    DbId = table.Column<int>()
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
                        "FK_CardBoxes_CardBoxSets_CardBoxSetDbId",
                        x => x.CardBoxSetDbId,
                        "CardBoxSets",
                        "DbId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "CardBoxSets_CompletedRevisionSessionIds",
                table => new
                {
                    CardBoxSetDbId = table.Column<int>(),
                    Id = table.Column<int>()
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<Guid>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardBoxSets_CompletedRevisionSessionIds", x => new {x.CardBoxSetDbId, x.Id});
                    table.ForeignKey(
                        "FK_CardBoxSets_CompletedRevisionSessionIds_CardBoxSets_CardBoxSetDbId",
                        x => x.CardBoxSetDbId,
                        "CardBoxSets",
                        "DbId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "SessionCards",
                table => new
                {
                    DbId = table.Column<int>()
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<Guid>(nullable: true),
                    CardId = table.Column<Guid>(nullable: true),
                    Status = table.Column<int>(),
                    NativeLanguageWord = table.Column<string>(nullable: true),
                    TargetLanguageWord = table.Column<string>(nullable: true),
                    RevisionSessionDbId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionCards", x => x.DbId);
                    table.ForeignKey(
                        "FK_SessionCards_RevisionSessions_RevisionSessionDbId",
                        x => x.RevisionSessionDbId,
                        "RevisionSessions",
                        "DbId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Domain.Cards",
                table => new
                {
                    DbId = table.Column<int>()
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
                        "FK_Cards_CardBoxes_CardBoxDbId",
                        x => x.CardBoxDbId,
                        "CardBoxes",
                        "DbId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_CardBoxes_CardBoxSetDbId",
                "CardBoxes",
                "CardBoxSetDbId");

            migrationBuilder.CreateIndex(
                "IX_Cards_CardBoxDbId",
                "Domain.Cards",
                "CardBoxDbId");

            migrationBuilder.CreateIndex(
                "IX_SessionCards_RevisionSessionDbId",
                "SessionCards",
                "RevisionSessionDbId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "CardBoxSets_CompletedRevisionSessionIds");

            migrationBuilder.DropTable(
                "Domain.Cards");

            migrationBuilder.DropTable(
                "SessionCards");

            migrationBuilder.DropTable(
                "CardBoxes");

            migrationBuilder.DropTable(
                "RevisionSessions");

            migrationBuilder.DropTable(
                "CardBoxSets");
        }
    }
}