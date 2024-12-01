using ViteMontevideo_API.Services.Dtos.Tarifas.Responses;
using ViteMontevideo_API.Services.Dtos.Vehiculos.Responses;

namespace ViteMontevideo_API.Services.Dtos.Servicios.Responses
{
    public class ServicioSalidaResponseDto
    {
        public int IdServicio { get; set; }
        public int IdCaja { get; set; }
        public DateTime FechaEntrada { get; set; }
        public TimeSpan HoraEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public string TipoPago { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public decimal Descuento { get; set; }
        public string? Observacion { get; set; }
        public bool EstadoPago { get; set; }
        public virtual VehiculoSimplificadoResponseDto Vehiculo { get; set; } = null!;
        public virtual TarifaResponseDto Tarifa { get; set; } = null!;
    }
}
