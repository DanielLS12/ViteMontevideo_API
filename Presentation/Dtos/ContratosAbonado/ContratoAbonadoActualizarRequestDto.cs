using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Presentation.Dtos.ContratosAbonado
{
    public class ContratoAbonadoActualizarRequestDto
    {
        public int? IdVehiculo { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFinal { get; set; }

        public TimeSpan? HoraInicio { get; set; }

        public TimeSpan? HoraFinal { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal? Monto { get; set; }

        public string? Observacion { get; set; }
    }
}
