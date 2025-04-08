using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentTransaction.Migrations
{
    /// <inheritdoc />
    public partial class FixTransactionPaymentMethodRelation2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Status_StatusId1",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_StatusId1",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "StatusId1",
                table: "Transaction");

            migrationBuilder.AlterColumn<Guid>(
                name: "StatusId",
                table: "Transaction",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_StatusId",
                table: "Transaction",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Status_StatusId",
                table: "Transaction",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Status_StatusId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_StatusId",
                table: "Transaction");

            migrationBuilder.AlterColumn<string>(
                name: "StatusId",
                table: "Transaction",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId1",
                table: "Transaction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_StatusId1",
                table: "Transaction",
                column: "StatusId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Status_StatusId1",
                table: "Transaction",
                column: "StatusId1",
                principalTable: "Status",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
