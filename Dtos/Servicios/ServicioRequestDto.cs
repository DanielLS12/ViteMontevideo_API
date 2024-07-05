using System.ComponentModel.DataAnnotations;
using ViteMontevideo_API.Models;

namespace ViteMontevideo_API.Dtos.Servicios
{
    public class ServicioRequestDto
    {
        public int IdVehiculo { get; set; }

        public short? IdTarifa { get; set; }

        public int? IdCaja { get; set; }

        public TimeSpan HoraEntrada { get; set; }

        public TimeSpan? HoraSalida { get; set; }

        public DateTime FechaEntrada { get; set; }

        public DateTime? FechaSalida { get; set; }

        [EnumDataType(typeof(TipoPago),ErrorMessage = "El tipo de pago ingresado no es válido.")]
        public string? TipoPago { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal Monto { get; set; }

        public decimal Descuento { get; set; }

        public string? Observacion { get; set; }

        public bool EstadoPago { get; set; }
    }
}
