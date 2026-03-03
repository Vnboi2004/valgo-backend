using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAlgo.Modules.ProblemClassification.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial_ProblemClassification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "problem_classification");

            migrationBuilder.CreateTable(
                name: "classifications",
                schema: "problem_classification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classifications", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "classifications",
                schema: "problem_classification");
        }
    }
}
