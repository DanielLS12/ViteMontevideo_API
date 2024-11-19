namespace ViteMontevideo_API.Presentation.Dtos.Servicios.Filtros
{
    public class FiltroServicio
    {
        public string? Placa { get; set; }
        public bool EstadoPago { get; set; } = false;
        public DateTime FechaInicio { get; set; } = DateTime.Today.AddDays(-1);
        public DateTime FechaFinal { get; set; } = DateTime.Today;
        public TimeSpan HoraInicio { get; set; } = TimeSpan.Zero;
        public TimeSpan HoraFinal { get; set; } = new TimeSpan(23, 59, 59);
        public string Orden { get; set; } = "desc";
    }
}
