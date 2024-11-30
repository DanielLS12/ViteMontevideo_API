using System.ComponentModel.DataAnnotations;
using ViteMontevideo_API.Helpers.Enums;

namespace ViteMontevideo_API.Presentation.Dtos.Servicios
{
    public class ServicioPagarRequestDto
    {
        public DateTime FechaSalida { get; set; }

        public TimeSpan HoraSalida { get; set; }

        [Required(ErrorMessage = "El campo tipo pago es requerido.")]
        [EnumDataType(typeof(TipoPago), ErrorMessage = "El tipo de pago ingresado no es válido.")]
        public string TipoPago { get; set; } = string.Empty;

        [Range(0.1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal Monto { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "El descuento no puede ser un número negativo.")]
        public decimal Descuento { get; set; }
    }
}
