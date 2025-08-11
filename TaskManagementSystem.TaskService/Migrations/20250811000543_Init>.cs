using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementSystem.TaskService.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    board_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assigned_to_id = table.Column<Guid>(type: "uuid", nullable: false),
                    column_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    rank = table.Column<long>(type: "bigint", nullable: false),
                    deadline = table.Column<long>(type: "bigint", nullable: false),
                    created_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<long>(type: "bigint", nullable: false),
                    updated_at = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tasks", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tasks_board_id",
                table: "tasks",
                column: "board_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasks_board_id_column_id",
                table: "tasks",
                columns: new[] { "board_id", "column_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tasks_column_id",
                table: "tasks",
                column: "column_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasks_rank",
                table: "tasks",
                column: "rank");

            migrationBuilder.CreateIndex(
                name: "ix_tasks_rank_board_id",
                table: "tasks",
                columns: new[] { "rank", "board_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tasks");
        }
    }
}
