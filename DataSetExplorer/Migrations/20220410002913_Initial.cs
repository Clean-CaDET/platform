using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DataSetExplorer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Annotators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    YearsOfExperience = table.Column<int>(type: "integer", nullable: false),
                    Ranking = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annotators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeSmells",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    DataSetId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeSmells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeSmells_DataSets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataSetProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<int>(type: "integer", nullable: false),
                    DataSetId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSetProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataSetProjects_DataSets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmellCandidateInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodeSmellId = table.Column<int>(type: "integer", nullable: true),
                    DataSetProjectId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmellCandidateInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmellCandidateInstances_CodeSmells_CodeSmellId",
                        column: x => x.CodeSmellId,
                        principalTable: "CodeSmells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmellCandidateInstances_DataSetProjects_DataSetProjectId",
                        column: x => x.DataSetProjectId,
                        principalTable: "DataSetProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataSetInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodeSnippetId = table.Column<string>(type: "text", nullable: true),
                    Link = table.Column<string>(type: "text", nullable: true),
                    ProjectLink = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    MetricFeatures = table.Column<string>(type: "text", nullable: true),
                    SmellCandidateInstancesId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSetInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataSetInstances_SmellCandidateInstances_SmellCandidateInst~",
                        column: x => x.SmellCandidateInstancesId,
                        principalTable: "SmellCandidateInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataSetAnnotations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InstanceSmellId = table.Column<int>(type: "integer", nullable: true),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    AnnotatorId = table.Column<int>(type: "integer", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    InstanceId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSetAnnotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataSetAnnotations_Annotators_AnnotatorId",
                        column: x => x.AnnotatorId,
                        principalTable: "Annotators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataSetAnnotations_CodeSmells_InstanceSmellId",
                        column: x => x.InstanceSmellId,
                        principalTable: "CodeSmells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataSetAnnotations_DataSetInstances_InstanceId",
                        column: x => x.InstanceId,
                        principalTable: "DataSetInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelatedInstance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodeSnippetId = table.Column<string>(type: "text", nullable: true),
                    Link = table.Column<string>(type: "text", nullable: true),
                    RelationType = table.Column<string>(type: "text", nullable: false),
                    CouplingTypeAndStrength = table.Column<string>(type: "text", nullable: true),
                    InstanceId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatedInstance_DataSetInstances_InstanceId",
                        column: x => x.InstanceId,
                        principalTable: "DataSetInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmellHeuristics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsApplicable = table.Column<bool>(type: "boolean", nullable: false),
                    ReasonForApplicability = table.Column<string>(type: "text", nullable: true),
                    AnnotationId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmellHeuristics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmellHeuristics_DataSetAnnotations_AnnotationId",
                        column: x => x.AnnotationId,
                        principalTable: "DataSetAnnotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeSmells_DataSetId",
                table: "CodeSmells",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSetAnnotations_AnnotatorId",
                table: "DataSetAnnotations",
                column: "AnnotatorId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSetAnnotations_InstanceId",
                table: "DataSetAnnotations",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSetAnnotations_InstanceSmellId",
                table: "DataSetAnnotations",
                column: "InstanceSmellId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSetInstances_SmellCandidateInstancesId",
                table: "DataSetInstances",
                column: "SmellCandidateInstancesId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSetProjects_DataSetId",
                table: "DataSetProjects",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedInstance_InstanceId",
                table: "RelatedInstance",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_SmellCandidateInstances_CodeSmellId",
                table: "SmellCandidateInstances",
                column: "CodeSmellId");

            migrationBuilder.CreateIndex(
                name: "IX_SmellCandidateInstances_DataSetProjectId",
                table: "SmellCandidateInstances",
                column: "DataSetProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SmellHeuristics_AnnotationId",
                table: "SmellHeuristics",
                column: "AnnotationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelatedInstance");

            migrationBuilder.DropTable(
                name: "SmellHeuristics");

            migrationBuilder.DropTable(
                name: "DataSetAnnotations");

            migrationBuilder.DropTable(
                name: "Annotators");

            migrationBuilder.DropTable(
                name: "DataSetInstances");

            migrationBuilder.DropTable(
                name: "SmellCandidateInstances");

            migrationBuilder.DropTable(
                name: "CodeSmells");

            migrationBuilder.DropTable(
                name: "DataSetProjects");

            migrationBuilder.DropTable(
                name: "DataSets");
        }
    }
}
