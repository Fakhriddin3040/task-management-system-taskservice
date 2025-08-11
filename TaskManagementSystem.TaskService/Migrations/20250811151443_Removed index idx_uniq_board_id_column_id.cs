using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementSystem.TaskService.Migrations
{
    /// <inheritdoc />
    public partial class Removedindexidx_uniq_board_id_column_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tasks_board_id_column_id",
                table: "tasks");

            migrationBuilder.CreateIndex(
                name: "ix_tasks_board_id_column_id",
                table: "tasks",
                columns: new[] { "board_id", "column_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tasks_board_id_column_id",
                table: "tasks");

            migrationBuilder.CreateIndex(
                name: "ix_tasks_board_id_column_id",
                table: "tasks",
                columns: new[] { "board_id", "column_id" },
                unique: true);
        }
    }
}
