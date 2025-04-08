using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentTransaction.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentMethodIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_PaymentMethod_PaymentMethodId1",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_PaymentMethodId1",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId1",
                table: "Transaction");

            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentMethodId",
                table: "Transaction",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_PaymentMethodId",
                table: "Transaction",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_PaymentMethod_PaymentMethodId",
                table: "Transaction",
                column: "PaymentMethodId",
                principalTable: "PaymentMethod",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_PaymentMethod_PaymentMethodId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_PaymentMethodId",
                table: "Transaction");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethodId",
                table: "Transaction",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentMethodId1",
                table: "Transaction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_PaymentMethodId1",
                table: "Transaction",
                column: "PaymentMethodId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_PaymentMethod_PaymentMethodId1",
                table: "Transaction",
                column: "PaymentMethodId1",
                principalTable: "PaymentMethod",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
