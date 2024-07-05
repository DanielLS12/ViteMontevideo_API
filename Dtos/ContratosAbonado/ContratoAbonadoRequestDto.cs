using System.ComponentModel.DataAnnotations;
using ViteMontevideo_API.Models;

namespace ViteMontevideo_API.Dtos.ContratosAbonado
{
    public class ContratoAbonadoRequestDto
    {
        public int IdVehiculo { get; set; }

        public int? IdCaja { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFinal { get; set; }

        public DateTime? FechaPago { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFinal { get; set; }

        public TimeSpan? HoraPago { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal Monto { get; set; }

        [EnumDataType(typeof(TipoPago), ErrorMessage = "El tipo de pago ingresado no es válido.")]
        public string? TipoPago { get; set; }

        public bool EstadoPago { get; set; }

        public string? Observacion { get; set; }
    }
}
