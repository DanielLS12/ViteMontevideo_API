using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Services.Dtos.Clientes.Requests
{
    public class ClienteActualizarRequestDto
    {
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$", ErrorMessage = "El campo nombres solo puede contener letras.")]
        public string Nombres { get; set; } = string.Empty;

        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$", ErrorMessage = "El campo apellidos solo puede contener letras.")]
        public string Apellidos { get; set; } = string.Empty;

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo telefono debe contener solo dígitos numéricos sin espacios ni caracteres especiales.")]
        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        public string? Correo { get; set; }
    }
}
