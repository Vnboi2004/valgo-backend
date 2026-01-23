using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAlgo.Modules.ProblemManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial_ProblemManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "problems");

            migrationBuilder.EnsureSchema(
                name: "problem_classifications");

            migrationBuilder.CreateTable(
                name: "problems",
                schema: "problems",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    statement = table.Column<string>(type: "text", nullable: false),
                    short_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    difficulty = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    time_limit_ms = table.Column<int>(type: "integer", nullable: false),
                    memory_limit_kb = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_problems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "problem_allowed_languages",
                schema: "problems",
                columns: table => new
                {
                    language = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    problem_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_problem_allowed_languages", x => new { x.problem_id, x.language });
                    table.ForeignKey(
                        name: "FK_problem_allowed_languages_problems_problem_id",
                        column: x => x.problem_id,
                        principalSchema: "problems",
                        principalTable: "problems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "problem_classifications",
                schema: "problem_classifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    classification_id = table.Column<Guid>(type: "uuid", nullable: false),
                    problem_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_problem_classifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_problem_classifications_problems_problem_id",
                        column: x => x.problem_id,
                        principalSchema: "problems",
                        principalTable: "problems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "problem_test_cases",
                schema: "problems",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    input = table.Column<string>(type: "text", nullable: false),
                    expected_output = table.Column<string>(type: "text", nullable: false),
                    output_comparion_strategy = table.Column<int>(type: "integer", nullable: false),
                    is_sample = table.Column<bool>(type: "boolean", nullable: false),
                    problem_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_problem_test_cases", x => x.id);
                    table.ForeignKey(
                        name: "FK_problem_test_cases_problems_problem_id",
                        column: x => x.problem_id,
                        principalSchema: "problems",
                        principalTable: "problems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_problem_classifications_problem_id",
                schema: "problem_classifications",
                table: "problem_classifications",
                column: "problem_id");

            migrationBuilder.CreateIndex(
                name: "IX_problem_test_cases_order",
                schema: "problems",
                table: "problem_test_cases",
                column: "order");

            migrationBuilder.CreateIndex(
                name: "IX_problem_test_cases_problem_id",
                schema: "problems",
                table: "problem_test_cases",
                column: "problem_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "problem_allowed_languages",
                schema: "problems");

            migrationBuilder.DropTable(
                name: "problem_classifications",
                schema: "problem_classifications");

            migrationBuilder.DropTable(
                name: "problem_test_cases",
                schema: "problems");

            migrationBuilder.DropTable(
                name: "problems",
                schema: "problems");
        }
    }
}
