using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Presentation.Dtos.Servicios
{
    public class ServicioActualizarRequestDto
    {
        [Required(ErrorMessage = "El campo placa vehicular es requerido.")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "La placa vehicular solo puede contener letras y números, sin espacios ni caracteres especiales.")]
        public string PlacaVehicular { get; set; } = string.Empty;

        public string? Observacion { get; set; }
    }
}
