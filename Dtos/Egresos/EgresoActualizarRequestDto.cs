using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Dtos.Egresos
{
    public class EgresoActualizarRequestDto
    {
        public string? Motivo { get; set; } = null!;

        [Range(0.1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal? Monto { get; set; }

        public DateTime? Fecha { get; set; }

        public TimeSpan? Hora { get; set; }
    }
}
