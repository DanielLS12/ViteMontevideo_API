using System.ComponentModel.DataAnnotations;
using ViteMontevideo_API.Services.Enums;

namespace ViteMontevideo_API.Services.Dtos.CajasChicas.Requests
{
    public class CajaChicaActualizarRequestDto
    {
        public short? IdTrabajador { get; set; }

        [EnumDataType(typeof(Turno), ErrorMessage = "El turno ingresado no es válido.")]
        public string? Turno { get; set; } = null!;

        [Range(0.0, double.MaxValue, ErrorMessage = "El saldo inicial no puede ser un número negativo.")]
        public decimal? SaldoInicial { get; set; }

        public string? Observacion { get; set; }
    }
}
