using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Memoyed.Application.Migrations
{
    public partial class CardBoxCurrentRevisionSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrentRevisionSessionId",
                table: "CardBoxSets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentRevisionSessionId",
                table: "CardBoxSets");
        }
    }
}
