using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAlgo.Modules.Contests.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Contests_V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "rank",
                table: "contest_participants",
                newName: "submission_count");

            migrationBuilder.RenameColumn(
                name: "last_submission_at",
                table: "contest_participants",
                newName: "registered_at");

            migrationBuilder.AddColumn<bool>(
                name: "allow_practice",
                table: "contests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "allow_virtual",
                table: "contests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "contests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "freeze_at",
                table: "contests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_leaderboard_frozen",
                table: "contests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "registration_end_time",
                table: "contests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "registration_start_time",
                table: "contests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "total_submissions",
                table: "contests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "contests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "contests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_registered",
                table: "contest_participants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "contest_submissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    contest_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    problem_id = table.Column<Guid>(type: "uuid", nullable: false),
                    verdict = table.Column<int>(type: "integer", nullable: false),
                    submitted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contest_submissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_contest_submissions_contests_contest_id",
                        column: x => x.contest_id,
                        principalTable: "contests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "virtual_contest_sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    contest_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: false),
                    penalty = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_virtual_contest_sessions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "participant_problem_stats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    contest_participant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    virtual_contest_session_id = table.Column<Guid>(type: "uuid", nullable: true),
                    problem_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_solved = table.Column<bool>(type: "boolean", nullable: false),
                    wrong_attempts = table.Column<int>(type: "integer", nullable: false),
                    solved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_participant_problem_stats", x => x.id);
                    table.ForeignKey(
                        name: "FK_participant_problem_stats_contest_participants_contest_part~",
                        column: x => x.contest_participant_id,
                        principalTable: "contest_participants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_participant_problem_stats_virtual_contest_sessions_virtual_~",
                        column: x => x.virtual_contest_session_id,
                        principalTable: "virtual_contest_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_contest_problems_contest_id_problem_id",
                table: "contest_problems",
                columns: new[] { "contest_id", "problem_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_contest_submissions_contest_id",
                table: "contest_submissions",
                column: "contest_id");

            migrationBuilder.CreateIndex(
                name: "IX_contest_submissions_contest_id_problem_id",
                table: "contest_submissions",
                columns: new[] { "contest_id", "problem_id" });

            migrationBuilder.CreateIndex(
                name: "IX_contest_submissions_contest_id_user_id",
                table: "contest_submissions",
                columns: new[] { "contest_id", "user_id" });

            migrationBuilder.CreateIndex(
                name: "IX_participant_problem_stats_contest_participant_id_problem_id",
                table: "participant_problem_stats",
                columns: new[] { "contest_participant_id", "problem_id" },
                unique: true,
                filter: "\"contest_participant_id\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_participant_problem_stats_virtual_contest_session_id_proble~",
                table: "participant_problem_stats",
                columns: new[] { "virtual_contest_session_id", "problem_id" },
                unique: true,
                filter: "\"virtual_contest_session_id\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_virtual_contest_sessions_contest_id",
                table: "virtual_contest_sessions",
                column: "contest_id");

            migrationBuilder.CreateIndex(
                name: "IX_virtual_contest_sessions_contest_id_user_id",
                table: "virtual_contest_sessions",
                columns: new[] { "contest_id", "user_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contest_submissions");

            migrationBuilder.DropTable(
                name: "participant_problem_stats");

            migrationBuilder.DropTable(
                name: "virtual_contest_sessions");

            migrationBuilder.DropIndex(
                name: "IX_contest_problems_contest_id_problem_id",
                table: "contest_problems");

            migrationBuilder.DropColumn(
                name: "allow_practice",
                table: "contests");

            migrationBuilder.DropColumn(
                name: "allow_virtual",
                table: "contests");

            migrationBuilder.DropColumn(
                name: "code",
                table: "contests");

            migrationBuilder.DropColumn(
                name: "freeze_at",
                table: "contests");

            migrationBuilder.DropColumn(
                name: "is_leaderboard_frozen",
                table: "contests");

            migrationBuilder.DropColumn(
                name: "registration_end_time",
                table: "contests");

            migrationBuilder.DropColumn(
                name: "registration_start_time",
                table: "contests");

            migrationBuilder.DropColumn(
                name: "total_submissions",
                table: "contests");

            migrationBuilder.DropColumn(
                name: "type",
                table: "contests");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "contests");

            migrationBuilder.DropColumn(
                name: "is_registered",
                table: "contest_participants");

            migrationBuilder.RenameColumn(
                name: "submission_count",
                table: "contest_participants",
                newName: "rank");

            migrationBuilder.RenameColumn(
                name: "registered_at",
                table: "contest_participants",
                newName: "last_submission_at");
        }
    }
}
