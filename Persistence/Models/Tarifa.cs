using System.ComponentModel.DataAnnotations.Schema;

namespace ViteMontevideo_API.Persistence.Models;

public partial class Tarifa
{
    public short IdTarifa { get; set; }
    public short IdCategoria { get; set; }
    public short IdActividad { get; set; }
    public decimal PrecioDia { get; set; }
    public decimal PrecioNoche { get; set; }
    [Column("hora_dia")]
    public TimeSpan? HoraDia { get; set; }
    [Column("hora_noche")]
    public TimeSpan? HoraNoche { get; set; }
    public TimeSpan Tolerancia { get; set; }
    public virtual Actividad Actividad { get; set; } = null!;
    public virtual Categoria Categoria { get; set; } = null!;
    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
