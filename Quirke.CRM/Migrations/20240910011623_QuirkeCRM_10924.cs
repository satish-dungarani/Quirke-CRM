using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quirke.CRM.Migrations
{
    /// <inheritdoc />
    public partial class QuirkeCRM_10924 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerRecords_Employees_AttendedEmployeeId",
                table: "CustomerRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerRecords_Masters_TreatmentId",
                table: "CustomerRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerRecords_Products_ProductId",
                table: "CustomerRecords");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Customers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "TreatmentId",
                table: "CustomerRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "CustomerRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FriendlyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Xml = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRecords_Employees_AttendedEmployeeId",
                table: "CustomerRecords",
                column: "AttendedEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRecords_Masters_TreatmentId",
                table: "CustomerRecords",
                column: "TreatmentId",
                principalTable: "Masters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRecords_Products_ProductId",
                table: "CustomerRecords",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerRecords_Employees_AttendedEmployeeId",
                table: "CustomerRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerRecords_Masters_TreatmentId",
                table: "CustomerRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerRecords_Products_ProductId",
                table: "CustomerRecords");

            migrationBuilder.DropTable(
                name: "DataProtectionKeys");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Customers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TreatmentId",
                table: "CustomerRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "CustomerRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRecords_Employees_AttendedEmployeeId",
                table: "CustomerRecords",
                column: "AttendedEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRecords_Masters_TreatmentId",
                table: "CustomerRecords",
                column: "TreatmentId",
                principalTable: "Masters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRecords_Products_ProductId",
                table: "CustomerRecords",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
