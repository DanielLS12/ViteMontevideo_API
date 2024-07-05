using System.ComponentModel.DataAnnotations;
using ViteMontevideo_API.Models;

namespace ViteMontevideo_API.Dtos.CajasChicas
{
    public class CajaChicaRequestDto
    {
        public short IdTrabajador { get; set; }

        [EnumDataType(typeof(Turno),ErrorMessage = "El turno ingresado no es válido.")]
        public string Turno { get; set; } = null!;

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFinal { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan? HoraFinal { get; set; }

        public decimal SaldoInicial { get; set; }

        public string? Observacion { get; set; }

        public bool Estado { get; set; }
    }
}
