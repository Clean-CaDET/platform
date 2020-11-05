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
                name: "EducationContents",
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
                    table.PrimaryKey("PK_EducationContents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EducationSnippets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SnippetQuality = table.Column<int>(nullable: false),
                    SnippetDifficulty = table.Column<int>(nullable: false),
                    SnippetType = table.Column<int>(nullable: false),
                    Tags = table.Column<List<Tag>>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    EducationContentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationSnippets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationSnippets_EducationContents_EducationContentId",
                        column: x => x.EducationContentId,
                        principalTable: "EducationContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationSnippets_EducationContentId",
                table: "EducationSnippets",
                column: "EducationContentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationSnippets");

            migrationBuilder.DropTable(
                name: "EducationContents");
        }
    }
}
