using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ViteMontevideo_API.Persistence.Models;
public partial class Categoria
{
    public short IdCategoria { get; set; }
    [Required(ErrorMessage = "El nombre es requerido.")]
    public string Nombre { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Tarifa> Tarifas { get; set; } = new List<Tarifa>();
}
