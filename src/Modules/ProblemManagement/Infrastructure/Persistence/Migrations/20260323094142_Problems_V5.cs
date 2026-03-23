using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAlgo.Modules.ProblemManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Problems_V5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "judge_template",
                schema: "problems",
                table: "problem_code_template",
                newName: "judge_template_header");

            migrationBuilder.AddColumn<string>(
                name: "judge_template_footer",
                schema: "problems",
                table: "problem_code_template",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "judge_template_footer",
                schema: "problems",
                table: "problem_code_template");

            migrationBuilder.RenameColumn(
                name: "judge_template_header",
                schema: "problems",
                table: "problem_code_template",
                newName: "judge_template");
        }
    }
}
