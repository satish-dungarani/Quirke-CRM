using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quirke.CRM.Migrations
{
    /// <inheritdoc />
    public partial class QuirkeCRM_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_CustomerRecords_Employees_EmployeeId",
            //    table: "CustomerRecords");

            //migrationBuilder.DropIndex(
            //    name: "IX_CustomerRecords_EmployeeId",
            //    table: "CustomerRecords");

            //migrationBuilder.DropColumn(
            //    name: "EmployeeId",
            //    table: "CustomerRecords");

            migrationBuilder.AlterColumn<string>(
                name: "DevTime",
                table: "CustomerRecords",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRecords_AttendedEmployeeId",
                table: "CustomerRecords",
                column: "AttendedEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRecords_Employees_AttendedEmployeeId",
                table: "CustomerRecords",
                column: "AttendedEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_CustomerRecords_Employees_AttendedEmployeeId",
            //    table: "CustomerRecords");

            //migrationBuilder.DropIndex(
            //    name: "IX_CustomerRecords_AttendedEmployeeId",
            //    table: "CustomerRecords");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "DevTime",
                table: "CustomerRecords",
                type: "time",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "CustomerRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRecords_EmployeeId",
                table: "CustomerRecords",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRecords_Employees_EmployeeId",
                table: "CustomerRecords",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
