using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Dtos.Trabajadores
{
    public class TrabajadorActualizarRequestDto
    {
        public byte? IdCargo { get; set; }

        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$", ErrorMessage = "El campo apellidos solo puede contener letras.")]
        public string? Nombre { get; set; } = null!;

        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$", ErrorMessage = "El campo apellidos solo puede contener letras.")]
        public string? ApellidoPaterno { get; set; } = null!;

        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$", ErrorMessage = "El campo apellidos solo puede contener letras.")]
        public string? ApellidoMaterno { get; set; } = null!;

        [EmailAddress(ErrorMessage = "No cumple con el formato.")]
        public string? Correo { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo telefono debe contener solo dígitos numéricos sin espacios ni caracteres especiales.")]
        public string? Telefono { get; set; }

        public string? Dni { get; set; } = null!;

        public bool? Estado { get; set; }
    }
}
