using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViteMontevideo_API.Migrations
{
    /// <inheritdoc />
    public partial class QuitarCampoFechaEgresos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fecha",
                table: "Egresos");
            }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "fecha",
                table: "Egresos",
                type: "date",
                nullable: false);
        }
    }
}
