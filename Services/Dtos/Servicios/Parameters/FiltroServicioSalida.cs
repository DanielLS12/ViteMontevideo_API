using ViteMontevideo_API.Services.Enums;

namespace ViteMontevideo_API.Services.Dtos.Servicios.Parameters
{
    public class FiltroServicioSalida
    {
        public string? Placa { get; set; }
        public DateTime FechaInicio { get; set; } = DateTime.Today.AddDays(-1);
        public DateTime FechaFinal { get; set; } = DateTime.Today;
        public TimeSpan HoraInicio { get; set; } = TimeSpan.Zero;
        public TimeSpan HoraFinal { get; set; } = new TimeSpan(23, 59, 59);
        public Orden Orden { get; set; }
    }
}
