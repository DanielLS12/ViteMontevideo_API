namespace ViteMontevideo_API.Persistence.Models;

public partial class Servicio
{
    public int IdServicio { get; set; }
    public int IdVehiculo { get; set; }
    public short? IdTarifa { get; set; }
    public int? IdCaja { get; set; }
    public TimeSpan HoraEntrada { get; set; }
    public TimeSpan? HoraSalida { get; set; }
    public DateTime FechaEntrada { get; set; }
    public DateTime? FechaSalida { get; set; }
    public string? TipoPago { get; set; }
    public decimal Monto { get; set; }
    public decimal Descuento { get; set; }
    public string? Observacion { get; set; }
    public bool EstadoPago { get; set; }
    public virtual CajaChica? CajaChica { get; set; } = null!;
    public virtual Vehiculo Vehiculo { get; set; } = null!;
    public virtual Tarifa? Tarifa { get; set; } = null!;
}
