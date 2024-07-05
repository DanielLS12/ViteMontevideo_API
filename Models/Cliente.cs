using System.ComponentModel.DataAnnotations.Schema;
using ViteMontevideo_API.models;

namespace ViteMontevideo_API.Models;
public partial class Cliente 
{ 
    [Column("id_cliente")]
    public int IdCliente { get; set; }
    public string Nombres { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public string? Telefono { get; set; }
    public string? Correo { get; set;}
    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
    public virtual ICollection<ComercioAdicional> ComerciosAdicinales { get; set; } = new List<ComercioAdicional>();
}
