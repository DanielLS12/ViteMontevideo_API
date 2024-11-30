using ViteMontevideo_API.Presentation.Dtos.Vehiculos;

namespace ViteMontevideo_API.Presentation.Dtos.Servicios
{
    public class ServicioEntradaResponseDto
    {
        public int IdServicio { get; set; }
        public DateTime FechaEntrada { get; set; }
        public TimeSpan HoraEntrada { get; set; }
        public decimal Monto { get; set; }
        public decimal Descuento { get; set; }
        public string? Observacion { get; set; }
        public bool EstadoPago { get; set; }
        public virtual VehiculoDetailResponseDto Vehiculo { get; set; } = null!;
    }
}
