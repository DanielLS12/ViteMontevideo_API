using ViteMontevideo_API.Dtos.Vehiculos;

namespace ViteMontevideo_API.Dtos.ContratosAbonado
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
        public virtual VehiculoSimplificadoDto Vehiculo { get; set; } = null!;
    }
}
