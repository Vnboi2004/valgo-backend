using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAlgo.Modules.Contests.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Contests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    visibility = table.Column<int>(type: "integer", nullable: false),
                    max_participants = table.Column<int>(type: "integer", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contest_participants",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    contest_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    joined_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: false),
                    penalty = table.Column<int>(type: "integer", nullable: false),
                    rank = table.Column<int>(type: "integer", nullable: false),
                    last_submission_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contest_participants", x => x.id);
                    table.ForeignKey(
                        name: "FK_contest_participants_contests_contest_id",
                        column: x => x.contest_id,
                        principalTable: "contests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contest_problems",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    contest_id = table.Column<Guid>(type: "uuid", nullable: false),
                    problem_id = table.Column<Guid>(type: "uuid", nullable: false),
                    problem_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    points = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contest_problems", x => x.id);
                    table.ForeignKey(
                        name: "FK_contest_problems_contests_contest_id",
                        column: x => x.contest_id,
                        principalTable: "contests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_contest_participants_contest_id",
                table: "contest_participants",
                column: "contest_id");

            migrationBuilder.CreateIndex(
                name: "IX_contest_participants_contest_id_user_id",
                table: "contest_participants",
                columns: new[] { "contest_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_contest_problems_contest_id",
                table: "contest_problems",
                column: "contest_id");

            migrationBuilder.CreateIndex(
                name: "IX_contest_problems_contest_id_order",
                table: "contest_problems",
                columns: new[] { "contest_id", "order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_contests_end_time",
                table: "contests",
                column: "end_time");

            migrationBuilder.CreateIndex(
                name: "IX_contests_start_time",
                table: "contests",
                column: "start_time");

            migrationBuilder.CreateIndex(
                name: "IX_contests_status",
                table: "contests",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contest_participants");

            migrationBuilder.DropTable(
                name: "contest_problems");

            migrationBuilder.DropTable(
                name: "contests");
        }
    }
}
