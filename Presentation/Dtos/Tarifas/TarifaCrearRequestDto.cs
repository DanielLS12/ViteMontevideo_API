using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Presentation.Dtos.Tarifas
{
    public class TarifaCrearRequestDto
    {
        [Required(ErrorMessage = "Elegir una categoria es requerido.")]
        public short IdCategoria { get; set; }

        [Required(ErrorMessage = "Elegir una actividad es requerido.")]
        public short IdActividad { get; set; }

        [Required(ErrorMessage = "El campo precio día es requerido.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal PrecioDia { get; set; }

        [Required(ErrorMessage = "El campo precio noche es requerido.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal PrecioNoche { get; set; }

        public TimeSpan? HoraDia { get; set; }

        public TimeSpan? HoraNoche { get; set; }

        public TimeSpan Tolerancia { get; set; }
    }
}
