using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAlgo.Modules.Submissions.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDomainSubmissionV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "submissions");

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
                    time_limit_ms = table.Column<int>(type: "integer", nullable: false),
                    memory_limit_kb = table.Column<int>(type: "integer", nullable: false),
                    retry_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    status = table.Column<int>(type: "integer", nullable: false),
                    verdict = table.Column<int>(type: "integer", nullable: false),
                    total_test_cases = table.Column<int>(type: "integer", nullable: true),
                    passed_test_cases = table.Column<int>(type: "integer", nullable: true),
                    max_time_ms = table.Column<int>(type: "integer", nullable: true),
                    max_memory_kb = table.Column<int>(type: "integer", nullable: true),
                    failure_reason = table.Column<int>(type: "integer", nullable: true),
                    worker_id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
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
                name: "submission_test_case_results",
                schema: "submissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    submission_id = table.Column<Guid>(type: "uuid", nullable: false),
                    test_case_index = table.Column<int>(type: "integer", nullable: false),
                    verdict = table.Column<int>(type: "integer", nullable: false),
                    time_ms = table.Column<int>(type: "integer", nullable: false),
                    memory_kb = table.Column<int>(type: "integer", nullable: false),
                    output = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_submission_test_case_results", x => x.id);
                    table.ForeignKey(
                        name: "FK_submission_test_case_results_submissions_submission_id",
                        column: x => x.submission_id,
                        principalSchema: "submissions",
                        principalTable: "submissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_test_case_results_submission",
                schema: "submissions",
                table: "submission_test_case_results",
                column: "submission_id");

            migrationBuilder.CreateIndex(
                name: "ux_test_case_results_submission_case",
                schema: "submissions",
                table: "submission_test_case_results",
                columns: new[] { "submission_id", "test_case_index" },
                unique: true);

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
                name: "ix_submissions_retry",
                schema: "submissions",
                table: "submissions",
                columns: new[] { "status", "retry_count" });

            migrationBuilder.CreateIndex(
                name: "ix_submissions_status_worker",
                schema: "submissions",
                table: "submissions",
                columns: new[] { "status", "worker_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "submission_test_case_results",
                schema: "submissions");

            migrationBuilder.DropTable(
                name: "submissions",
                schema: "submissions");
        }
    }
}
