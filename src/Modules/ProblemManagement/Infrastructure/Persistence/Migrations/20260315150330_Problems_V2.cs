using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAlgo.Modules.ProblemManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Problems_V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "problem_classifications",
                schema: "problem_classifications",
                newName: "problem_classifications",
                newSchema: "problems");

            migrationBuilder.AddColumn<string>(
                name: "constraints",
                schema: "problems",
                table: "problems",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "editorial",
                schema: "problems",
                table: "problems",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "follow_up",
                schema: "problems",
                table: "problems",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "input_format",
                schema: "problems",
                table: "problems",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "output_format",
                schema: "problems",
                table: "problems",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "companies",
                schema: "problems",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "problem_companies",
                schema: "problems",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    problem_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_problem_companies", x => x.id);
                    table.ForeignKey(
                        name: "FK_problem_companies_problems_problem_id",
                        column: x => x.problem_id,
                        principalSchema: "problems",
                        principalTable: "problems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "problem_examples",
                schema: "problems",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    input = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    output = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    explanation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    problem_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_problem_examples", x => x.id);
                    table.ForeignKey(
                        name: "FK_problem_examples_problems_problem_id",
                        column: x => x.problem_id,
                        principalSchema: "problems",
                        principalTable: "problems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "problem_hints",
                schema: "problems",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    problem_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_problem_hints", x => x.id);
                    table.ForeignKey(
                        name: "FK_problem_hints_problems_problem_id",
                        column: x => x.problem_id,
                        principalSchema: "problems",
                        principalTable: "problems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "similar_problems",
                schema: "problems",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    similar_problem_id = table.Column<Guid>(type: "uuid", nullable: false),
                    problem_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_similar_problems", x => x.id);
                    table.ForeignKey(
                        name: "FK_similar_problems_problems_problem_id",
                        column: x => x.problem_id,
                        principalSchema: "problems",
                        principalTable: "problems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_companies_name",
                schema: "problems",
                table: "companies",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_problem_companies_problem_id",
                schema: "problems",
                table: "problem_companies",
                column: "problem_id");

            migrationBuilder.CreateIndex(
                name: "IX_problem_examples_problem_id",
                schema: "problems",
                table: "problem_examples",
                column: "problem_id");

            migrationBuilder.CreateIndex(
                name: "IX_problem_hints_problem_id",
                schema: "problems",
                table: "problem_hints",
                column: "problem_id");

            migrationBuilder.CreateIndex(
                name: "IX_similar_problems_problem_id",
                schema: "problems",
                table: "similar_problems",
                column: "problem_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "companies",
                schema: "problems");

            migrationBuilder.DropTable(
                name: "problem_companies",
                schema: "problems");

            migrationBuilder.DropTable(
                name: "problem_examples",
                schema: "problems");

            migrationBuilder.DropTable(
                name: "problem_hints",
                schema: "problems");

            migrationBuilder.DropTable(
                name: "similar_problems",
                schema: "problems");

            migrationBuilder.DropColumn(
                name: "constraints",
                schema: "problems",
                table: "problems");

            migrationBuilder.DropColumn(
                name: "editorial",
                schema: "problems",
                table: "problems");

            migrationBuilder.DropColumn(
                name: "follow_up",
                schema: "problems",
                table: "problems");

            migrationBuilder.DropColumn(
                name: "input_format",
                schema: "problems",
                table: "problems");

            migrationBuilder.DropColumn(
                name: "output_format",
                schema: "problems",
                table: "problems");

            migrationBuilder.EnsureSchema(
                name: "problem_classifications");

            migrationBuilder.RenameTable(
                name: "problem_classifications",
                schema: "problems",
                newName: "problem_classifications",
                newSchema: "problem_classifications");
        }
    }
}
