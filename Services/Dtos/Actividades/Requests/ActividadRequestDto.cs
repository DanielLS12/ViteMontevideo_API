using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Services.Dtos.Actividades.Requests
{
    public class ActividadRequestDto
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
