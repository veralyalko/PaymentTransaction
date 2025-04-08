using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentTransaction.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderTypeAndMetadataJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetadataJson",
                table: "Provider",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Provider",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetadataJson",
                table: "Provider");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Provider");
        }
    }
}
