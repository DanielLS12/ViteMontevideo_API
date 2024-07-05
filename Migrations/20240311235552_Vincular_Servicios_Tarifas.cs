using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViteMontevideo_API.Migrations
{
    /// <inheritdoc />
    public partial class Vincular_Servicios_Tarifas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "id_tarifa",
                table: "Servicios",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_id_tarifa",
                table: "Servicios",
                column: "id_tarifa");

            migrationBuilder.AddForeignKey(
                name: "Fk_Servicios_Tarifas",
                table: "Servicios",
                column: "id_tarifa",
                principalTable: "Tarifas",
                principalColumn: "id_tarifa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Fk_Servicios_Tarifas",
                table: "Servicios");

            migrationBuilder.DropIndex(
                name: "IX_Servicios_id_tarifa",
                table: "Servicios");

            migrationBuilder.DropColumn(
                name: "id_tarifa",
                table: "Servicios");
        }
    }
}
