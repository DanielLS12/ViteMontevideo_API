using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViteMontevideo_API.Migrations
{
    /// <inheritdoc />
    public partial class Agregar_Tablas_Clientes__ComerciosAdicionales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_cliente",
                table: "Vehiculos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombres = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    apellidos = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    telefono = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    correo = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cliente__WD54B5TA4FD54BE5", x => x.id_cliente);
                });

            migrationBuilder.CreateTable(
                name: "Comercios_Adicionales",
                columns: table => new
                {
                    id_comercio_adicional = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_cliente = table.Column<int>(type: "int", nullable: false),
                    tipo_comercio_adicional = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    monto = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ComercioAdicinal__4202OZRAM3021M", x => x.id_comercio_adicional);
                    table.ForeignKey(
                        name: "Fk_ComerciosAdicionales_Clientes",
                        column: x => x.id_cliente,
                        principalTable: "Clientes",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_id_cliente",
                table: "Vehiculos",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_ComercioAdicional_id_cliente",
                table: "Comercios_Adicionales",
                column: "id_cliente");

            migrationBuilder.AddForeignKey(
                name: "Fk_Vehiculos_Clientes",
                table: "Vehiculos",
                column: "id_cliente",
                principalTable: "Clientes",
                principalColumn: "id_cliente",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Fk_Vehiculos_Clientes",
                table: "Vehiculos");

            migrationBuilder.DropTable(
                name: "Comercios_Adicionales");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Vehiculos_id_cliente",
                table: "Vehiculos");

            migrationBuilder.DropIndex(
                name: "IX_ComercioAdicional_id_cliente",
                table: "Comercios_Adicionales");

            migrationBuilder.DropColumn(
                name: "id_cliente",
                table: "Vehiculos");
        }
    }
}
