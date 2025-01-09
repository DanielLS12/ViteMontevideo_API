using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViteMontevideo_API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarMontosCajaChica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "total_monto_abonados",
                table: "Cajas_Chicas",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total_monto_comercios_adicionales",
                table: "Cajas_Chicas",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total_monto_egresos",
                table: "Cajas_Chicas",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total_monto_servicios",
                table: "Cajas_Chicas",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_monto_abonados",
                table: "Cajas_Chicas");

            migrationBuilder.DropColumn(
                name: "total_monto_comercios_adicionales",
                table: "Cajas_Chicas");

            migrationBuilder.DropColumn(
                name: "total_monto_egresos",
                table: "Cajas_Chicas");

            migrationBuilder.DropColumn(
                name: "total_monto_servicios",
                table: "Cajas_Chicas");
        }
    }
}
