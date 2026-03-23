using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAlgo.Modules.ProblemManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Problems_V4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "problem_code_template",
                schema: "problems",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    language = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    user_template = table.Column<string>(type: "text", nullable: false),
                    judge_template = table.Column<string>(type: "text", nullable: false),
                    problem_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_problem_code_template", x => x.id);
                    table.ForeignKey(
                        name: "FK_problem_code_template_problems_problem_id",
                        column: x => x.problem_id,
                        principalSchema: "problems",
                        principalTable: "problems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_problem_code_template_problem_id",
                schema: "problems",
                table: "problem_code_template",
                column: "problem_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "problem_code_template",
                schema: "problems");
        }
    }
}
