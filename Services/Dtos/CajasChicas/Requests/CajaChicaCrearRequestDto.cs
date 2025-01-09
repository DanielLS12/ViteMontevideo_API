using System.ComponentModel.DataAnnotations;
using ViteMontevideo_API.Services.Enums;

namespace ViteMontevideo_API.Services.Dtos.CajasChicas.Requests
{
    public class CajaChicaCrearRequestDto
    {
        public short IdTrabajador { get; set; }

        [Required(ErrorMessage = "El campo turno es requerido.")]
        [EnumDataType(typeof(Turno), ErrorMessage = "El turno ingresado no es válido.")]
        public string Turno { get; set; } = null!;

        public DateTime FechaInicio { get; set; }

        public TimeSpan HoraInicio { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "El saldo inicial no puede ser un número negativo.")]
        public decimal SaldoInicial { get; set; }
    }
}
