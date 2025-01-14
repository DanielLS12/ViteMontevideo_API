using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Services.Dtos.Cargos.Requests
{
    public class CargoRequestDto
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
