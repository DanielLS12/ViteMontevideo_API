﻿using System.ComponentModel.DataAnnotations;
using ViteMontevideo_API.Helpers.Enums;

namespace ViteMontevideo_API.Presentation.Dtos.ComerciosAdicionales
{
    public class ComercioAdicionalPagarRequestDto
    {
        public DateTime FechaPago { get; set; }

        public TimeSpan HoraPago { get; set; }

        [Required(ErrorMessage = "El campo tipo pago es requerido.")]
        [EnumDataType(typeof(TipoPago), ErrorMessage = "El tipo de pago ingresado no es válido.")]
        public string TipoPago { get; set; } = string.Empty;
    }
}
