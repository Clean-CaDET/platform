using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SmartTutor.ContentModel;

namespace SmartTutor.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EducationalContents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentQuality = table.Column<int>(nullable: false),
                    ContentDifficulty = table.Column<int>(nullable: false),
                    EducationSnippetsIds = table.Column<List<int>>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationalContents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EducationalSnippets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SnippetQuality = table.Column<int>(nullable: false),
                    SnippetDifficulty = table.Column<int>(nullable: false),
                    SnippetType = table.Column<int>(nullable: false),
                    Tags = table.Column<List<Tag>>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    EducationalContentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationalSnippets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationalSnippets_EducationalContents_EducationalContentId",
                        column: x => x.EducationalContentId,
                        principalTable: "EducationalContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationalSnippets_EducationalContentId",
                table: "EducationalSnippets",
                column: "EducationalContentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationalSnippets");

            migrationBuilder.DropTable(
                name: "EducationalContents");
        }
    }
}
