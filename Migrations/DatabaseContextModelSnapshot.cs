﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using repassAPI.Data;

#nullable disable

namespace repassAPI.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("repassAPI.Entities.AccessToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("token");

                    b.HasKey("Id");

                    b.ToTable("banned_tokens");
                });

            modelBuilder.Entity("repassAPI.Entities.CampusGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("groups");
                });

            modelBuilder.Entity("repassAPI.Entities.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Annotations")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("annotations");

                    b.Property<Guid>("CampusGroupId")
                        .HasColumnType("uuid")
                        .HasColumnName("group_id");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("create_time");

                    b.Property<Guid?>("MainTeacherId")
                        .HasColumnType("uuid")
                        .HasColumnName("main_teacher_id");

                    b.Property<int>("MaxStudentsCount")
                        .HasColumnType("integer")
                        .HasColumnName("max_students_count");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("RemainingSlotsCount")
                        .HasColumnType("integer")
                        .HasColumnName("remaining_slots_count");

                    b.Property<string>("Requirements")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("requirements");

                    b.Property<int>("Semester")
                        .HasColumnType("integer")
                        .HasColumnName("semester");

                    b.Property<int>("StartYear")
                        .HasColumnType("integer")
                        .HasColumnName("start_year");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.HasIndex("CampusGroupId");

                    b.HasIndex("MainTeacherId");

                    b.ToTable("courses");
                });

            modelBuilder.Entity("repassAPI.Entities.CourseStudent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid")
                        .HasColumnName("course_id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("student_email");

                    b.Property<int>("FinalResult")
                        .HasColumnType("integer")
                        .HasColumnName("final_result");

                    b.Property<int>("MidtermResult")
                        .HasColumnType("integer")
                        .HasColumnName("midterm_result");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("student_name");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("student_status");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid")
                        .HasColumnName("student_id");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("course_student");
                });

            modelBuilder.Entity("repassAPI.Entities.CourseTeacher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid")
                        .HasColumnName("course_id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("student_email");

                    b.Property<bool>("IsMainTeacher")
                        .HasColumnType("boolean")
                        .HasColumnName("is_main_teacher");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("student_name");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uuid")
                        .HasColumnName("teacher_id");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("TeacherId");

                    b.ToTable("course_teacher");
                });

            modelBuilder.Entity("repassAPI.Entities.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid")
                        .HasColumnName("course_id");

                    b.Property<bool>("IsImportant")
                        .HasColumnType("boolean")
                        .HasColumnName("is_important");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("notifications");
                });

            modelBuilder.Entity("repassAPI.Entities.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expires");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("token");

                    b.HasKey("Id");

                    b.ToTable("refresh_tokens");
                });

            modelBuilder.Entity("repassAPI.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("birth_date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("full_name");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean")
                        .HasColumnName("is_admin");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("repassAPI.Entities.UserRoles", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("IsStudent")
                        .HasColumnType("boolean")
                        .HasColumnName("is_student");

                    b.Property<bool>("IsTeacher")
                        .HasColumnType("boolean")
                        .HasColumnName("is_teacher");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("user_roles");
                });

            modelBuilder.Entity("repassAPI.Entities.Course", b =>
                {
                    b.HasOne("repassAPI.Entities.CampusGroup", "CampusGroup")
                        .WithMany("Courses")
                        .HasForeignKey("CampusGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("repassAPI.Entities.User", "MainTeacher")
                        .WithMany()
                        .HasForeignKey("MainTeacherId");

                    b.Navigation("CampusGroup");

                    b.Navigation("MainTeacher");
                });

            modelBuilder.Entity("repassAPI.Entities.CourseStudent", b =>
                {
                    b.HasOne("repassAPI.Entities.Course", "Course")
                        .WithMany("Students")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("repassAPI.Entities.User", "Student")
                        .WithMany("StudingCourses")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("repassAPI.Entities.CourseTeacher", b =>
                {
                    b.HasOne("repassAPI.Entities.Course", "Course")
                        .WithMany("Teachers")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("repassAPI.Entities.User", "Teacher")
                        .WithMany("TeachingCourses")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("repassAPI.Entities.Notification", b =>
                {
                    b.HasOne("repassAPI.Entities.Course", "Course")
                        .WithMany("Notifications")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("repassAPI.Entities.UserRoles", b =>
                {
                    b.HasOne("repassAPI.Entities.User", "User")
                        .WithOne()
                        .HasForeignKey("repassAPI.Entities.UserRoles", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("repassAPI.Entities.CampusGroup", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("repassAPI.Entities.Course", b =>
                {
                    b.Navigation("Notifications");

                    b.Navigation("Students");

                    b.Navigation("Teachers");
                });

            modelBuilder.Entity("repassAPI.Entities.User", b =>
                {
                    b.Navigation("StudingCourses");

                    b.Navigation("TeachingCourses");
                });
#pragma warning restore 612, 618
        }
    }
}
