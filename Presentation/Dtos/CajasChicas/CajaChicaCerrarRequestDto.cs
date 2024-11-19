using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Presentation.Dtos.CajasChicas
{
    public class CajaChicaCerrarRequestDto
    {
        [Required(ErrorMessage = "El campo fecha final es requerido.")]
        public DateTime? FechaFinal { get; set; }

        [Required(ErrorMessage = "El campo hora final es requerido.")]
        public TimeSpan? HoraFinal { get; set; }

        public string? Observacion { get; set; }
    }
}
