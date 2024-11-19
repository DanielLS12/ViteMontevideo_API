using System.ComponentModel.DataAnnotations;
using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Presentation.Dtos.ComerciosAdicionales
{
    public class ComercioAdicionalActualizarRequestDto
    {
        public int? IdCliente { get; set; }

        [EnumDataType(typeof(TipoComercioAdicional), ErrorMessage = "El tipo de coemrcio adicional ingresado no es válido.")]
        public string? TipoComercioAdicional { get; set; } = null!;

        [Range(0.1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal? Monto { get; set; }

        public string? Observacion { get; set; }
    }
}
