using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Dtos.Servicios
{
    public class ServicioCrearRequestDto
    {
        [Required(ErrorMessage = "Elegir un vehículo es requerido.")]
        public int? IdVehiculo { get; set; }

        [Required(ErrorMessage = "La fecha entrada es requerida.")]
        public DateTime? FechaEntrada { get; set; }

        [Required(ErrorMessage = "La hora entrada es requerida.")]
        public TimeSpan? HoraEntrada { get; set; }
    }
}
