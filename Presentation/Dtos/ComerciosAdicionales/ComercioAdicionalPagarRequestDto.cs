using System.ComponentModel.DataAnnotations;
using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Presentation.Dtos.ComerciosAdicionales
{
    public class ComercioAdicionalPagarRequestDto
    {
        [Required(ErrorMessage = "El campo fecha pago es requerido.")]
        public DateTime? FechaPago { get; set; }

        [Required(ErrorMessage = "El campo hora pago es requerido.")]
        public TimeSpan? HoraPago { get; set; }

        [Required(ErrorMessage = "El campo tipo pago es requerido.")]
        [EnumDataType(typeof(TipoPago), ErrorMessage = "El tipo de pago ingresado no es válido.")]
        public string? TipoPago { get; set; }
    }
}
