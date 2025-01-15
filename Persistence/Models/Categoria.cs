namespace ViteMontevideo_API.Persistence.Models;

public partial class Categoria
{
    public short IdCategoria { get; set; }
    public string Nombre { get; set; } = null!;
    public virtual ICollection<Tarifa> Tarifas { get; set; } = new List<Tarifa>();
}
