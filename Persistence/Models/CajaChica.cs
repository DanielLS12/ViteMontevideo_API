namespace ViteMontevideo_API.Persistence.Models;

public partial class CajaChica
{
    public int IdCaja { get; set; }
    public short IdTrabajador { get; set; }
    public string Turno { get; set; } = null!;
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFinal { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan? HoraFinal { get; set; }
    public decimal SaldoInicial { get; set; }
    public string? Observacion { get; set; }
    public bool Estado { get; set; }
    public virtual Trabajador Trabajador { get; set; } = null!;
    public virtual ICollection<Egreso> Egresos { get; set; } = new List<Egreso>();
    public virtual ICollection<ContratoAbonado> ContratosAbonados { get; set; } = new List<ContratoAbonado>();
    public virtual ICollection<ComercioAdicional> ComerciosAdicionales { get; set; } = new List<ComercioAdicional>();
    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
