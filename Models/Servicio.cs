﻿namespace ViteMontevideo_API.models;

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
    public virtual CajaChica? OCajaChica { get; set; } = null!;
    public virtual Vehiculo OVehiculo { get; set; } = null!;
    public virtual Tarifa? OTarifa { get; set; } = null!;
}
