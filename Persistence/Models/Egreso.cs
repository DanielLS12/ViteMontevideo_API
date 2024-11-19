namespace ViteMontevideo_API.Persistence.Models;

public partial class Egreso
{
    public int IdEgreso { get; set; }
    public int IdCaja { get; set; }
    public string Motivo { get; set; } = null!;
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan Hora { get; set; }
    public virtual CajaChica CajaChica { get; set; } = null!;
}
