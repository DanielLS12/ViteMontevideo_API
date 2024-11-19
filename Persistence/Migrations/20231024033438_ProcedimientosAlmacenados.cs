using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViteMontevideo_API.Migrations
{
    /// <inheritdoc />
    public partial class ProcedimientosAlmacenados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE dbo.Acceder
                                @Nombre VARCHAR(200),
                                @Clave VARCHAR(200)
                                AS
                                BEGIN
                                SELECT * FROM Usuarios WHERE nombre = @Nombre collate SQL_Latin1_General_CP1_CS_AS and CONVERT(VARCHAR(MAX), DECRYPTBYPASSPHRASE('AvDlFg2@23_622220', clave)) = @Clave collate SQL_Latin1_General_CP1_CS_AS
                                END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE dbo.Acceder");
        }
    }
}
