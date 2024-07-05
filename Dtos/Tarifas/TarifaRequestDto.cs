using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Dtos.Tarifas
{
    public class TarifaRequestDto
    {
        public short IdCategoria { get; set; }

        public short IdActividad { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal PrecioDia { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal PrecioNoche { get; set; }

        public TimeSpan? HoraDia { get; set; }

        public TimeSpan? HoraNoche { get; set; }

        public TimeSpan Tolerancia { get; set; }
    }
}
