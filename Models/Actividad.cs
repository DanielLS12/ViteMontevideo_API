using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ViteMontevideo_API.models;

public partial class Actividad
{
    public short IdActividad { get; set; }
    [Required(ErrorMessage = "El nombre es requerido.")]
    public string Nombre { get; set; } = string.Empty;
    [JsonIgnore]
    public virtual ICollection<Tarifa> Tarifas { get; set; } = new List<Tarifa>();
}
