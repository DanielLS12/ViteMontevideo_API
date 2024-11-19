using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViteMontevideo_API.Migrations
{
    /// <inheritdoc />
    public partial class Agregar_ForeignKey_ComerciosAdicionales_IdCaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_caja",
                table: "Comercios_Adicionales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comercios_Adicionales_id_caja",
                table: "Comercios_Adicionales",
                column: "id_caja");

            migrationBuilder.AddForeignKey(
                name: "Fk_ComerciosAdicionales_CajasChicas",
                table: "Comercios_Adicionales",
                column: "id_caja",
                principalTable: "Cajas_Chicas",
                principalColumn: "id_caja");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Fk_ComerciosAdicionales_CajasChicas",
                table: "Comercios_Adicionales");

            migrationBuilder.DropIndex(
                name: "IX_Comercios_Adicionales_id_caja",
                table: "Comercios_Adicionales");

            migrationBuilder.DropColumn(
                name: "id_caja",
                table: "Comercios_Adicionales");
        }
    }
}
