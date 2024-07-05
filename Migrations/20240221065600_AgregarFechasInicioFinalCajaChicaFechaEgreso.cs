using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViteMontevideo_API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFechasInicioFinalCajaChicaFechaEgreso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "fecha",
                table: "Cajas_Chicas",
                newName: "fecha_inicio");

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha",
                table: "Egresos",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_final",
                table: "Cajas_Chicas",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fecha",
                table: "Egresos");

            migrationBuilder.DropColumn(
                name: "fecha_final",
                table: "Cajas_Chicas");

            migrationBuilder.RenameColumn(
                name: "fecha_inicio",
                table: "Cajas_Chicas",
                newName: "fecha");
        }
    }
}
