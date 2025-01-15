namespace ViteMontevideo_API.Persistence.Models;

public partial class Actividad
{
    public short IdActividad { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public virtual ICollection<Tarifa> Tarifas { get; set; } = new List<Tarifa>();
}
