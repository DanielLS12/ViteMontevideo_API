using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViteMontevideo_API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTriggersServicios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TRIGGER after_update_recalcular_servicios
                ON Servicios
                AFTER UPDATE
                AS
                BEGIN
                    UPDATE Cajas_Chicas
                    SET total_monto_servicios = (
                        SELECT COALESCE(SUM(s.monto), 0)
                        FROM Servicios s
                        WHERE s.id_caja = Cajas_Chicas.id_caja
                    )
                    WHERE id_caja IN (
                        SELECT d.id_caja
                        FROM deleted d
                        LEFT JOIN inserted i ON d.id_servicio = i.id_servicio
                        WHERE 
                            d.id_caja IS NOT NULL AND (
                                ISNULL(d.monto, 0) <> ISNULL(i.monto, 0) OR
                                ISNULL(d.id_caja, -1) <> ISNULL(i.id_caja, -1)
                            )
                        UNION
                        SELECT i.id_caja
                        FROM inserted i
                        LEFT JOIN deleted d ON i.id_servicio = d.id_servicio
                        WHERE 
                            i.id_caja IS NOT NULL AND (
                                ISNULL(d.monto, 0) <> ISNULL(i.monto, 0) OR
                                ISNULL(d.id_caja, -1) <> ISNULL(i.id_caja, -1)
                            )
                    );
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER after_delete_recalcular_servicios
                ON Servicios
                AFTER delete
                AS
                BEGIN
                    UPDATE Cajas_Chicas
                    SET total_monto_servicios = (
                        SELECT COALESCE(SUM(s.monto),0)
                        FROM Servicios s
                        WHERE s.id_caja = Cajas_Chicas.id_caja
                    )
                    WHERE id_caja IN (
                        SELECT DISTINCT d.id_caja
                        FROM deleted d
                    );
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER after_update_recalcular_servicios");

            migrationBuilder.Sql("DROP TRIGGER after_delete_recalcular_servicios");
        }
    }
}
