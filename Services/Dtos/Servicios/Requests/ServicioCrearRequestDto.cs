using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Services.Dtos.Servicios.Requests
{
    public class ServicioCrearRequestDto
    {
        [Required(ErrorMessage = "El campo placa vehicular es requerido.")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "La placa vehicular solo puede contener letras y números, sin espacios ni caracteres especiales.")]
        public string PlacaVehicular { get; set; } = string.Empty;

        public DateTime FechaEntrada { get; set; }

        public TimeSpan HoraEntrada { get; set; }
    }
}
