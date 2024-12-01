using System.ComponentModel.DataAnnotations;
using ViteMontevideo_API.Services.Enums;

namespace ViteMontevideo_API.Services.Dtos.CajasChicas.Requests
{
    public class CajaChicaCrearRequestDto
    {
        [Required(ErrorMessage = "Elegir un trabajador es requerido.")]
        public short? IdTrabajador { get; set; }

        [Required(ErrorMessage = "El campo turno es requerido.")]
        [EnumDataType(typeof(Turno), ErrorMessage = "El turno ingresado no es válido.")]
        public string Turno { get; set; } = null!;

        [Required(ErrorMessage = "El campo fecha inicio es requerido.")]
        public DateTime? FechaInicio { get; set; }

        [Required(ErrorMessage = "El campo hora inicio es requerido.")]
        public TimeSpan? HoraInicio { get; set; }

        [Required(ErrorMessage = "El campo saldo inicial es requerido.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "El saldo inicial no puede ser un número negativo.")]
        public decimal? SaldoInicial { get; set; }

    }
}
