namespace ViteMontevideo_API.models;

public partial class ContratoAbonado
{
    public int IdContratoAbonado { get; set; }
    public int IdVehiculo { get; set; }
    public int? IdCaja { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFinal { get; set; }
    public DateTime? FechaPago { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFinal { get; set; }
    public TimeSpan? HoraPago { get; set; }
    public decimal Monto { get; set; }
    public string? TipoPago { get; set; }
    public bool EstadoPago { get; set; }
    public string? Observacion { get; set; }
    public virtual Vehiculo OVehiculo { get; set; } = null!;
    public virtual CajaChica? OCajaChica { get; set; } = null!;
}
