using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace repassAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_roles_user_id",
                table: "user_roles");

            migrationBuilder.AddColumn<string>(
                name: "annotations",
                table: "courses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "main_teacher_id",
                table: "courses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "requirements",
                table: "courses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "student_email",
                table: "course_teacher",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "student_name",
                table: "course_teacher",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "final_result",
                table: "course_student",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "midterm_result",
                table: "course_student",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "student_email",
                table: "course_student",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "student_name",
                table: "course_student",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    is_important = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_notifications_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_user_id",
                table: "user_roles",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_courses_main_teacher_id",
                table: "courses",
                column: "main_teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_course_id",
                table: "notifications",
                column: "course_id");

            migrationBuilder.AddForeignKey(
                name: "FK_courses_users_main_teacher_id",
                table: "courses",
                column: "main_teacher_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courses_users_main_teacher_id",
                table: "courses");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropIndex(
                name: "IX_user_roles_user_id",
                table: "user_roles");

            migrationBuilder.DropIndex(
                name: "IX_courses_main_teacher_id",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "annotations",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "main_teacher_id",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "requirements",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "student_email",
                table: "course_teacher");

            migrationBuilder.DropColumn(
                name: "student_name",
                table: "course_teacher");

            migrationBuilder.DropColumn(
                name: "final_result",
                table: "course_student");

            migrationBuilder.DropColumn(
                name: "midterm_result",
                table: "course_student");

            migrationBuilder.DropColumn(
                name: "student_email",
                table: "course_student");

            migrationBuilder.DropColumn(
                name: "student_name",
                table: "course_student");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_user_id",
                table: "user_roles",
                column: "user_id");
        }
    }
}
