﻿using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Services.Dtos.Trabajadores.Responses
{
    public class TrabajadorResponseDto
    {
        public short IdTrabajador { get; set; }
        public string Nombre { get; set; } = null!;
        public string ApellidoPaterno { get; set; } = null!;
        public string ApellidoMaterno { get; set; } = null!;
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public string Dni { get; set; } = null!;
        public bool Estado { get; set; }
        public virtual Cargo Cargo { get; set; } = null!;
    }
}
