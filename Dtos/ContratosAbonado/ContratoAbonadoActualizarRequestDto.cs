using System.ComponentModel.DataAnnotations;
using ViteMontevideo_API.Models;

namespace ViteMontevideo_API.Dtos.ContratosAbonado
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
