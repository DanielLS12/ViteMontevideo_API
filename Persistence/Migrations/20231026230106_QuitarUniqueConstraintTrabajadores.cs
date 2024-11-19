using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViteMontevideo_API.Migrations
{
    /// <inheritdoc />
    public partial class QuitarUniqueConstraintTrabajadores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "UQ__Trabajad__2A16D94509C064B4",
                table: "Trabajadores"
            );

            migrationBuilder.DropUniqueConstraint(
               name: "UQ__Trabajad__2A586E0BFAA6BA1B",
               table: "Trabajadores"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
