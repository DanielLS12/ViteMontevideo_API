using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Services.Dtos.CajasChicas.Requests
{
    public class CajaChicaCerrarRequestDto
    {
        public DateTime FechaFinal { get; set; }

        public TimeSpan HoraFinal { get; set; }

        public string? Observacion { get; set; }
    }
}
