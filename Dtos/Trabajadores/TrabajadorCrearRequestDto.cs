using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Dtos.Trabajadores
{
    public class TrabajadorCrearRequestDto
    {
        [Required(ErrorMessage = "Elegir un cargo es requerido.")]
        public byte? IdCargo { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$", ErrorMessage = "El campo apellidos solo puede contener letras.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El apellido paterno es requerido.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$", ErrorMessage = "El campo apellidos solo puede contener letras.")]
        public string ApellidoPaterno { get; set; } = null!;

        [Required(ErrorMessage = "El apellido materno es requerido.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$", ErrorMessage = "El campo apellidos solo puede contener letras.")]
        public string ApellidoMaterno { get; set; } = null!;

        [EmailAddress(ErrorMessage = "No cumple con el formato.")]
        public string? Correo { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo telefono debe contener solo dígitos numéricos sin espacios ni caracteres especiales.")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El DNI es requerido.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener exactamente 8 dígitos.")]
        public string Dni { get; set; } = null!;
    }
}
