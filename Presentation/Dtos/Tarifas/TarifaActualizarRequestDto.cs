using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Presentation.Dtos.Tarifas
{
    public class TarifaActualizarRequestDto
    {
        [Range(0.1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal PrecioDia { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal PrecioNoche { get; set; }

        public TimeSpan? HoraDia { get; set; }

        public TimeSpan? HoraNoche { get; set; }

        public TimeSpan Tolerancia { get; set; }
    }
}
