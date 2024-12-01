using ViteMontevideo_API.Services.Dtos.Vehiculos.Responses;

namespace ViteMontevideo_API.Services.Dtos.ContratosAbonado.Responses
{
    public class ContratoAbonadoResponseDto
    {
        public int IdContratoAbonado { get; set; }
        public int? IdCaja { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public DateTime? FechaPago { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFinal { get; set; }
        public TimeSpan? HoraPago { get; set; }
        public decimal Monto { get; set; }
        public string? TipoPago { get; set; }
        public bool EstadoPago { get; set; }
        public string? Observacion { get; set; }
        public virtual VehiculoSimplificadoResponseDto Vehiculo { get; set; } = null!;
    }
}
