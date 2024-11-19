using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViteMontevideo_API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCampos_FechaPago_HoraPago__ComerciosAdicionales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_pago",
                table: "Comercios_Adicionales",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "hora_pago",
                table: "Comercios_Adicionales",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fecha_pago",
                table: "Comercios_Adicionales");

            migrationBuilder.DropColumn(
                name: "hora_pago",
                table: "Comercios_Adicionales");
        }
    }
}
