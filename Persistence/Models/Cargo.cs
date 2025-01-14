namespace ViteMontevideo_API.Persistence.Models;

public partial class Cargo
{
    public byte IdCargo { get; set; }
    public string Nombre { get; set; } = null!;
    public virtual ICollection<Trabajador> Trabajadores { get; set; } = new List<Trabajador>();
}
