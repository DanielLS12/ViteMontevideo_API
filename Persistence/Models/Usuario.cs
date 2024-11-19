using System.Text.Json.Serialization;

namespace ViteMontevideo_API.Persistence.Models;

public partial class Usuario
{
    public short IdUsuario { get; set; }
    public string Nombre { get; set; } = null!;
    [JsonIgnore]
    public byte[] Clave { get; set; } = null!;
    public bool Estado { get; set; }
    [JsonIgnore]
    public virtual ICollection<Trabajador> Trabajadores { get; set; } = new List<Trabajador>();
}
