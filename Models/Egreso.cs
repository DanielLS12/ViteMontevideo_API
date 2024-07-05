using System.Text.Json.Serialization;

namespace ViteMontevideo_API.models;

public partial class Egreso
{
    public int IdEgreso { get; set; }
    public int IdCaja { get; set; }
    public string Motivo { get; set; } = null!;
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan Hora { get; set; }
    [JsonIgnore]
    public virtual CajaChica OCajaChica { get; set; } = null!;
}
