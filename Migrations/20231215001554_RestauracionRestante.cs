using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViteMontevideo_API.Migrations
{
    /// <inheritdoc />
    public partial class RestauracionRestante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "id_caja",
                table: "Servicios",
                nullable: true
                );

            migrationBuilder.AddColumn<bool>(
                name: "estado_pago",
                table: "Servicios",
                type: "bit",
                nullable: false,
                defaultValueSql: "((0))");

            migrationBuilder.AlterColumn<TimeOnly>(
               name: "hora_final",
               table: "Cajas_Chicas",
               nullable: true
               );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "estado_pago",
                table: "Servicios");
        }
    }
}
