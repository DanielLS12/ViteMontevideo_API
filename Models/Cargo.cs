using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ViteMontevideo_API.models;

public partial class Cargo
{
    public byte IdCargo { get; set; }
    [Required(ErrorMessage = "El nombre es requerido.")]
    public string Nombre { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Trabajador> Trabajadores { get; set; } = new List<Trabajador>();
}
