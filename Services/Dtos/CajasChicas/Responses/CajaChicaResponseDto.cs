using ViteMontevideo_API.Services.Dtos.Trabajadores.Responses;

namespace ViteMontevideo_API.Services.Dtos.CajasChicas.Responses
{
    public class CajaChicaResponseDto
    {
        public int IdCaja { get; set; }
        public string Turno { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan? HoraFinal { get; set; }
        public decimal SaldoInicial { get; set; }
        public string? Observacion { get; set; }
        public bool Estado { get; set; }
        public virtual TrabajadorResponseDto Trabajador { get; set; } = null!;
    }
}
