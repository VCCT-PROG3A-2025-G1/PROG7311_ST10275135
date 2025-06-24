using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriEnergy.Migrations
{
    /// <inheritdoc />
    public partial class AddProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Farmers_FarmerId1",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FarmerId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FarmerId1",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Products",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "FarmerId",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Products_FarmerId",
                table: "Products",
                column: "FarmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Farmers_FarmerId",
                table: "Products",
                column: "FarmerId",
                principalTable: "Farmers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Farmers_FarmerId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FarmerId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "FarmerId",
                table: "Products",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "FarmerId1",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FarmerId1",
                table: "Products",
                column: "FarmerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Farmers_FarmerId1",
                table: "Products",
                column: "FarmerId1",
                principalTable: "Farmers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
