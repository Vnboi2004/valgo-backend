using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAlgo.Modules.Submissions.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Submissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "submissions");

            migrationBuilder.EnsureSchema(
                name: "test_case_results");

            migrationBuilder.CreateTable(
                name: "submissions",
                schema: "submissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    problem_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    source_code = table.Column<string>(type: "text", nullable: false),
                    source_code_hash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    verdict = table.Column<int>(type: "integer", nullable: false),
                    total_test_cases = table.Column<int>(type: "integer", nullable: true),
                    passed_test_cases = table.Column<int>(type: "integer", nullable: true),
                    max_time_ms = table.Column<int>(type: "integer", nullable: true),
                    max_memory_kb = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    queued_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    finished_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_submissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "test_case_results",
                schema: "test_case_results",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    submission_id = table.Column<Guid>(type: "uuid", nullable: false),
                    index = table.Column<int>(type: "integer", nullable: false),
                    passed = table.Column<bool>(type: "boolean", nullable: false),
                    time_ms = table.Column<int>(type: "integer", nullable: false),
                    memory_kb = table.Column<int>(type: "integer", nullable: false),
                    error = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_case_results", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_submissions_created_at",
                schema: "submissions",
                table: "submissions",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "idx_submissions_problem_id",
                schema: "submissions",
                table: "submissions",
                column: "problem_id");

            migrationBuilder.CreateIndex(
                name: "idx_submissions_status",
                schema: "submissions",
                table: "submissions",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_submissions_user_id",
                schema: "submissions",
                table: "submissions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_test_case_results_submission_id",
                schema: "test_case_results",
                table: "test_case_results",
                column: "submission_id");

            migrationBuilder.CreateIndex(
                name: "uq_test_case_results_submission_id_test_case_index",
                schema: "test_case_results",
                table: "test_case_results",
                columns: new[] { "submission_id", "index" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "submissions",
                schema: "submissions");

            migrationBuilder.DropTable(
                name: "test_case_results",
                schema: "test_case_results");
        }
    }
}
