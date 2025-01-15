using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Services.Dtos.Categorias.Requests
{
    public class CategoriaRequestDto
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Nombre { get; set; } = null!;
    }
}
