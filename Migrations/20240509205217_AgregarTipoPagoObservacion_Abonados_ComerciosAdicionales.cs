using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViteMontevideo_API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTipoPagoObservacion_Abonados_ComerciosAdicionales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "observacion",
                table: "Contratos_Abonados",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tipo_pago",
                table: "Contratos_Abonados",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "observacion",
                table: "Comercios_Adicionales",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tipo_pago",
                table: "Comercios_Adicionales",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "observacion",
                table: "Contratos_Abonados");

            migrationBuilder.DropColumn(
                name: "tipo_pago",
                table: "Contratos_Abonados");

            migrationBuilder.DropColumn(
                name: "observacion",
                table: "Comercios_Adicionales");

            migrationBuilder.DropColumn(
                name: "tipo_pago",
                table: "Comercios_Adicionales");
        }
    }
}
