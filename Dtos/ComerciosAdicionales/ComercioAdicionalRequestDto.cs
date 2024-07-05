using System.ComponentModel.DataAnnotations;
using ViteMontevideo_API.Models;

namespace ViteMontevideo_API.Dtos.ComerciosAdicionales
{
    public class ComercioAdicionalRequestDto
    {
        public int? IdCaja { get; set; }

        public int IdCliente { get; set; }

        public string TipoComercioAdicional { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal Monto { get; set; }

        public DateTime? FechaPago { get; set; }

        public TimeSpan? HoraPago { get; set; }

        [EnumDataType(typeof(TipoPago), ErrorMessage = "El tipo de pago ingresado no es válido.")]
        public string? TipoPago { get; set; }

        public string? Observacion { get; set; }
    }
}
