using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace repassAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isAdmin",
                table: "users",
                newName: "is_admin");

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    start_year = table.Column<int>(type: "integer", nullable: false),
                    max_students_count = table.Column<int>(type: "integer", nullable: false),
                    remaining_slots_count = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    semester = table.Column<int>(type: "integer", nullable: false),
                    group_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.id);
                    table.ForeignKey(
                        name: "FK_Courses_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_group_id",
                table: "Courses",
                column: "group_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.RenameColumn(
                name: "is_admin",
                table: "users",
                newName: "isAdmin");
        }
    }
}
