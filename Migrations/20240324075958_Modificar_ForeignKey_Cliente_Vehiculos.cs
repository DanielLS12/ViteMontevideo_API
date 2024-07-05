using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViteMontevideo_API.Migrations
{
    /// <inheritdoc />
    public partial class Modificar_ForeignKey_Cliente_Vehiculos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Fk_Vehiculos_Clientes",
                table: "Vehiculos");


            migrationBuilder.AddForeignKey(
                name: "Fk_Vehiculos_Clientes",
                table: "Vehiculos",
                column: "id_cliente",
                principalTable: "Clientes",
                principalColumn: "id_cliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Fk_Vehiculos_Clientes",
                table: "Vehiculos");


            migrationBuilder.AddForeignKey(
                name: "Fk_Vehiculos_Clientes",
                table: "Vehiculos",
                column: "id_cliente",
                principalTable: "Cliente",
                principalColumn: "IdCliente",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
