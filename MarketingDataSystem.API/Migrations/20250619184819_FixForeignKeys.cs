using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingDataSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Productos_IdProducto",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Ventas_Clientes_IdCliente",
                table: "Ventas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ventas_Productos_IdProducto",
                table: "Ventas");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Productos_IdProducto",
                table: "Stocks",
                column: "IdProducto",
                principalTable: "Productos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ventas_Clientes_IdCliente",
                table: "Ventas",
                column: "IdCliente",
                principalTable: "Clientes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ventas_Productos_IdProducto",
                table: "Ventas",
                column: "IdProducto",
                principalTable: "Productos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Productos_IdProducto",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Ventas_Clientes_IdCliente",
                table: "Ventas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ventas_Productos_IdProducto",
                table: "Ventas");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Productos_IdProducto",
                table: "Stocks",
                column: "IdProducto",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ventas_Clientes_IdCliente",
                table: "Ventas",
                column: "IdCliente",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ventas_Productos_IdProducto",
                table: "Ventas",
                column: "IdProducto",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
