using System.Text.Json.Serialization;

namespace ViteMontevideo_API.Persistence.Models;

public partial class Trabajador
{
    public short IdTrabajador { get; set; }
    public short? IdUsuario { get; set; }
    public byte IdCargo { get; set; }
    public string Nombre { get; set; } = null!;
    public string ApellidoPaterno { get; set; } = null!;
    public string ApellidoMaterno { get; set; } = null!;
    public string? Correo { get; set; }
    public string? Telefono { get; set; }
    public string Dni { get; set; } = null!;
    public bool Estado { get; set; }
    public virtual ICollection<CajaChica> CajasChicas { get; set; } = new List<CajaChica>();
    public virtual Cargo Cargo { get; set; } = null!;
    public virtual Usuario? OUsuario { get; set; }
}
