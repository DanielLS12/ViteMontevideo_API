using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViteMontevideo_API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarHoraFechaPagoIdCajaRelacionAbonadosCaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "fecha_pago",
                table: "Contratos_Abonados",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "hora_pago",
                table: "Contratos_Abonados",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_caja",
                table: "Contratos_Abonados",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_Abonados_IdCaja",
                table: "Contratos_Abonados",
                column: "id_caja");

            migrationBuilder.AddForeignKey(
                name: "Fk_Contratos_Abonados_Cajas_Chicas",
                table: "Contratos_Abonados",
                column: "id_caja",
                principalTable: "Cajas_Chicas",
                principalColumn: "id_caja");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
