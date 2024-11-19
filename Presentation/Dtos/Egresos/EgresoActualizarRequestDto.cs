using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Presentation.Dtos.Egresos
{
    public class EgresoActualizarRequestDto
    {
        [Required(ErrorMessage = "El id de la caja chica es requerida.")]
        public int IdCajaChica { get; set; }

        [Required(ErrorMessage = "El campo motivo es requerido.")]
        public string Motivo { get; set; } = null!;

        [Range(0.1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El campo hora es requerido.")]
        public TimeSpan Hora { get; set; }
    }
}
