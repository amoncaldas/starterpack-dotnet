using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace starterpack.Migrations
{
    public partial class ChangeSheduledToInTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ScheduledTo",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ScheduledTo",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));
        }
    }
}
