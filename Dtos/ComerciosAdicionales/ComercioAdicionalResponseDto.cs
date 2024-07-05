using ViteMontevideo_API.Dtos.Clientes;

namespace ViteMontevideo_API.Dtos.ComerciosAdicionales
{
    public class ComercioAdicionalResponseDto
    {
        public int IdComercioAdicional { get; set; }
        public int? IdCaja { get; set; }
        public string TipoComercioAdicional { get; set; } = null!;
        public decimal Monto { get; set; }
        public DateTime? FechaPago { get; set; }
        public TimeSpan? HoraPago { get; set; }
        public string? TipoPago { get; set; }
        public string? Observacion { get; set; }
        public virtual ClienteDto Cliente { get; set; } = null!;
    }
}
