using System.ComponentModel.DataAnnotations;
using ViteMontevideo_API.Models;

namespace ViteMontevideo_API.Dtos.ComerciosAdicionales
{
    public class ComercioAdicionalCrearRequestDto
    {
        [Required(ErrorMessage = "Elegir un cliente es requerido.")]
        public int? IdCliente { get; set; }

        [Required(ErrorMessage = "El campo tipo comercio adicional es requerido.")]
        [EnumDataType(typeof(TipoComercioAdicional), ErrorMessage = "El tipo de comercio adicional ingresado no es válido.")]
        public string? TipoComercioAdicional { get; set; } = null!;

        [Required(ErrorMessage = "El campo monto es requerido.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal Monto { get; set; }
    }
}
