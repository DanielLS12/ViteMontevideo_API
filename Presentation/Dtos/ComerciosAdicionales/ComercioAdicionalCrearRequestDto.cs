using System.ComponentModel.DataAnnotations;
using ViteMontevideo_API.Helpers.Enums;

namespace ViteMontevideo_API.Presentation.Dtos.ComerciosAdicionales
{
    public class ComercioAdicionalCrearRequestDto
    {
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El campo tipo comercio adicional es requerido.")]
        [EnumDataType(typeof(TipoComercioAdicional), ErrorMessage = "El tipo de comercio adicional ingresado no es válido.")]
        public string TipoComercioAdicional { get; set; } = null!;

        [Range(0.1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal Monto { get; set; }
    }
}
