using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ViteMontevideo_API.models;

namespace ViteMontevideo_API.Models;

[Table("Comercios_Adicionales")]
public partial class ComercioAdicional
{
    [Key]
    [Column("id_comercio_adicional")]
    public int IdComercioAdicional { get; set; }
    public int? IdCaja { get; set; }
    [Column("id_cliente")]
    public int IdCliente { get; set; }
    [Column("tipo_comercio_adicional")]
    public string TipoComercioAdicional { get; set; } = null!;
    public decimal Monto { get; set; }
    public DateTime? FechaPago { get; set; }
    public TimeSpan? HoraPago { get; set; }
    public string? TipoPago { get; set; }
    public string? Observacion { get; set; }
    public virtual Cliente Cliente { get; set; } = null!;
    public virtual CajaChica? CajaChica { get; set; } = null!;
}

