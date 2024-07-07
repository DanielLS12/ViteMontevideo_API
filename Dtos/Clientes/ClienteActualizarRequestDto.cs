using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Dtos.Clientes
{
    public class ClienteActualizarRequestDto
    {
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$", ErrorMessage = "El campo nombres solo puede contener letras.")]
        public string? Nombres { get; set; }

        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$", ErrorMessage = "El campo apellidos solo puede contener letras.")]
        public string? Apellidos { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo telefono debe contener solo dígitos numéricos sin espacios ni caracteres especiales.")]
        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        public string? Correo { get; set; }
    }
}
