using System.ComponentModel.DataAnnotations;
using ViteMontevideo_API.Models;

namespace ViteMontevideo_API.Dtos.Servicios
{
    public class ServicioActualizarRequestDto
    {
        [Required(ErrorMessage = "Elegir un vehículo es requerido.")]
        public int? IdVehiculo { get; set; }

        [EnumDataType(typeof(TipoPago), ErrorMessage = "El tipo de pago ingresado no es válido.")]
        public string? TipoPago { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "El descuento no puede ser un número negativo.")]
        public decimal? Descuento { get; set; }

        public string? Observacion { get; set; }
    }
}
