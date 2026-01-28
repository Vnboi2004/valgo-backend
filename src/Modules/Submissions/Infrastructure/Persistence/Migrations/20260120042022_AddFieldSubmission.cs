using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAlgo.Modules.Submissions.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                schema: "submissions",
                table: "submissions",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "memory_limit_kb",
                schema: "submissions",
                table: "submissions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "time_limit_ms",
                schema: "submissions",
                table: "submissions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailureReason",
                schema: "submissions",
                table: "submissions");

            migrationBuilder.DropColumn(
                name: "memory_limit_kb",
                schema: "submissions",
                table: "submissions");

            migrationBuilder.DropColumn(
                name: "time_limit_ms",
                schema: "submissions",
                table: "submissions");
        }
    }
}
