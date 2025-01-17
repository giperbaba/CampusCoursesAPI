﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace repassAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixedCreateTimeToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "create_time",
                table: "courses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "create_time",
                table: "courses");
        }
    }
}
