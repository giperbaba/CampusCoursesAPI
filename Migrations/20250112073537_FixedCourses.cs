using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace repassAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixedCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_groups_group_id",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "courses");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_group_id",
                table: "courses",
                newName: "IX_courses_group_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_courses",
                table: "courses",
                column: "id");

            migrationBuilder.CreateTable(
                name: "course_student",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_student", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_student_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_student_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_teacher",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    teacher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_main_teacher = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_teacher", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_teacher_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_teacher_users_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_course_student_course_id",
                table: "course_student",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_student_student_id",
                table: "course_student",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_teacher_course_id",
                table: "course_teacher",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_teacher_teacher_id",
                table: "course_teacher",
                column: "teacher_id");

            migrationBuilder.AddForeignKey(
                name: "FK_courses_groups_group_id",
                table: "courses",
                column: "group_id",
                principalTable: "groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courses_groups_group_id",
                table: "courses");

            migrationBuilder.DropTable(
                name: "course_student");

            migrationBuilder.DropTable(
                name: "course_teacher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_courses",
                table: "courses");

            migrationBuilder.RenameTable(
                name: "courses",
                newName: "Courses");

            migrationBuilder.RenameIndex(
                name: "IX_courses_group_id",
                table: "Courses",
                newName: "IX_Courses_group_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_groups_group_id",
                table: "Courses",
                column: "group_id",
                principalTable: "groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
