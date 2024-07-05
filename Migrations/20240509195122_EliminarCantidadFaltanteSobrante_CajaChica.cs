using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViteMontevideo_API.Migrations
{
    /// <inheritdoc />
    public partial class EliminarCantidadFaltanteSobrante_CajaChica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cantidad_sobrante_faltante",
                table: "Cajas_Chicas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "cantidad_sobrante_faltante",
                table: "Cajas_Chicas",
                type: "smallmoney",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
