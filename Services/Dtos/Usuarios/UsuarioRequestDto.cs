﻿using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Services.Dtos.Usuarios
{
    public class UsuarioRequestDto
    {
        [Required(ErrorMessage = "El usuario es requerido.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La clave es requerida.")]
        public string Clave { get; set; } = null!;
    }
}
