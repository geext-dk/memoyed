using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Memoyed.Application.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "card_box_sets",
                table => new
                {
                    db_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id = table.Column<Guid>(nullable: true),
                    current_revision_session_id = table.Column<Guid>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    native_language = table.Column<string>(nullable: true),
                    target_language = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("pk_card_box_sets", x => x.db_id); });

            migrationBuilder.CreateTable(
                "revision_sessions",
                table => new
                {
                    db_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id = table.Column<Guid>(nullable: true),
                    card_box_set_id = table.Column<Guid>(nullable: true),
                    status = table.Column<int>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("pk_revision_sessions", x => x.db_id); });

            migrationBuilder.CreateTable(
                "users",
                table => new
                {
                    db_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id = table.Column<Guid>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("pk_users", x => x.db_id); });

            migrationBuilder.CreateTable(
                "card_box_sets_completed_revision_session_ids",
                table => new
                {
                    card_box_set_db_id = table.Column<int>(nullable: false),
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    value = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_card_box_sets_completed_revision_session_ids",
                        x => new {x.card_box_set_db_id, x.id});
                    table.ForeignKey(
                        "fk_card_box_sets_completed_revision_session_ids_card_box_sets_",
                        x => x.card_box_set_db_id,
                        "card_box_sets",
                        "db_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "card_boxes",
                table => new
                {
                    db_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id = table.Column<Guid>(nullable: true),
                    set_id = table.Column<Guid>(nullable: true),
                    level = table.Column<int>(nullable: true),
                    revision_delay = table.Column<int>(nullable: true),
                    card_box_set_db_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_card_boxes", x => x.db_id);
                    table.ForeignKey(
                        "fk_card_boxes_card_box_sets_card_box_set_db_id",
                        x => x.card_box_set_db_id,
                        "card_box_sets",
                        "db_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "session_cards",
                table => new
                {
                    db_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    session_id = table.Column<Guid>(nullable: true),
                    card_id = table.Column<Guid>(nullable: true),
                    status = table.Column<int>(nullable: false),
                    native_language_word = table.Column<string>(nullable: true),
                    target_language_word = table.Column<string>(nullable: true),
                    revision_session_db_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_session_cards", x => x.db_id);
                    table.ForeignKey(
                        "fk_session_cards_revision_sessions_revision_session_db_id",
                        x => x.revision_session_db_id,
                        "revision_sessions",
                        "db_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "cards",
                table => new
                {
                    db_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id = table.Column<Guid>(nullable: true),
                    card_box_id = table.Column<Guid>(nullable: true),
                    native_language_word = table.Column<string>(nullable: true),
                    target_language_word = table.Column<string>(nullable: true),
                    comment = table.Column<string>(nullable: true),
                    card_box_changed_date = table.Column<DateTime>(nullable: true),
                    card_box_db_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cards", x => x.db_id);
                    table.ForeignKey(
                        "fk_cards_card_boxes_card_box_db_id",
                        x => x.card_box_db_id,
                        "card_boxes",
                        "db_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "ix_card_boxes_card_box_set_db_id",
                "card_boxes",
                "card_box_set_db_id");

            migrationBuilder.CreateIndex(
                "ix_cards_card_box_db_id",
                "cards",
                "card_box_db_id");

            migrationBuilder.CreateIndex(
                "ix_session_cards_revision_session_db_id",
                "session_cards",
                "revision_session_db_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "card_box_sets_completed_revision_session_ids");

            migrationBuilder.DropTable(
                "cards");

            migrationBuilder.DropTable(
                "session_cards");

            migrationBuilder.DropTable(
                "users");

            migrationBuilder.DropTable(
                "card_boxes");

            migrationBuilder.DropTable(
                "revision_sessions");

            migrationBuilder.DropTable(
                "card_box_sets");
        }
    }
}