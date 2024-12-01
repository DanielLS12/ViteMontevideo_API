using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Services.Dtos.ContratosAbonado.Requests
{
    public class ContratoAbonadoActualizarRequestDto
    {
        public DateTime FechaInicio { get; set; }

        public DateTime FechaFinal { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFinal { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal Monto { get; set; }

        public string? Observacion { get; set; }
    }
}
