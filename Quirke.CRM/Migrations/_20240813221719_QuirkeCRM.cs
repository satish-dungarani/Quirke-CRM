using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quirke.CRM.Migrations
{
    /// <inheritdoc />
    public partial class QuirkeCRM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_CustomerRecords_Customers_CustomerId",
            //    table: "CustomerRecords");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_CustomerRecords_Customers_CustomerId1",
            //    table: "CustomerRecords");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_CustomerRecords_Employees_AttendedEmployeeId",
            //    table: "CustomerRecords");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_CustomerRecords_Masters_TreatmentId",
            //    table: "CustomerRecords");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_CustomerRecords_Masters_TreatmentId1",
            //    table: "CustomerRecords");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_CustomerRecords_Products_ProductId",
            //    table: "CustomerRecords");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_CustomerRecords_Products_ProductId1",
            //    table: "CustomerRecords");

            //migrationBuilder.DropIndex(
            //    name: "IX_CustomerRecords_AttendedEmployeeId",
            //    table: "CustomerRecords");

            //migrationBuilder.DropIndex(
            //    name: "IX_CustomerRecords_CustomerId1",
            //    table: "CustomerRecords");

            //migrationBuilder.DropIndex(
            //    name: "IX_CustomerRecords_ProductId1",
            //    table: "CustomerRecords");

            //migrationBuilder.DropIndex(
            //    name: "IX_CustomerRecords_TreatmentId1",
            //    table: "CustomerRecords");

            //migrationBuilder.DropColumn(
            //    name: "CustomerId1",
            //    table: "CustomerRecords");

            //migrationBuilder.DropColumn(
            //    name: "ProductId1",
            //    table: "CustomerRecords");

            //migrationBuilder.DropColumn(
            //    name: "TreatmentId1",
            //    table: "CustomerRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRecords_Customers_CustomerId",
                table: "CustomerRecords",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerRecords_Customers_CustomerId",
                table: "CustomerRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerRecords_Masters_TreatmentId",
                table: "CustomerRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerRecords_Products_ProductId",
                table: "CustomerRecords");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId1",
                table: "CustomerRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                table: "CustomerRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TreatmentId1",
                table: "CustomerRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRecords_AttendedEmployeeId",
                table: "CustomerRecords",
                column: "AttendedEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRecords_CustomerId1",
                table: "CustomerRecords",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRecords_ProductId1",
                table: "CustomerRecords",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRecords_TreatmentId1",
                table: "CustomerRecords",
                column: "TreatmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRecords_Customers_CustomerId",
                table: "CustomerRecords",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRecords_Customers_CustomerId1",
                table: "CustomerRecords",
                column: "CustomerId1",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRecords_Employees_AttendedEmployeeId",
                table: "CustomerRecords",
                column: "AttendedEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRecords_Masters_TreatmentId",
                table: "CustomerRecords",
                column: "TreatmentId",
                principalTable: "Masters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRecords_Masters_TreatmentId1",
                table: "CustomerRecords",
                column: "TreatmentId1",
                principalTable: "Masters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRecords_Products_ProductId",
                table: "CustomerRecords",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRecords_Products_ProductId1",
                table: "CustomerRecords",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
