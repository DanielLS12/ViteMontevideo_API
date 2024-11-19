using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Presentation.Dtos.Egresos
{
    public class EgresoCrearRequestDto
    {
        [Required(ErrorMessage = "El campo motivo es requerido.")]
        public string Motivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo monto es requerido.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El campo fecha es requerido.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El campo hora es requerido.")]
        public TimeSpan Hora { get; set; }
    }
}
