using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Services.Dtos.ContratosAbonado.Requests
{
    public class ContratoAbonadoCrearRequestDto
    {
        [Required(ErrorMessage = "El campo placa vehicular es requerido.")]
        public string PlacaVehicular { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo fecha inicio es requerido.")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "El campo fecha final es requerido.")]
        public DateTime FechaFinal { get; set; }

        [Required(ErrorMessage = "El campo hora inicio es requerido.")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "El campo hora final es requerido.")]
        public TimeSpan HoraFinal { get; set; }

        [Required(ErrorMessage = "El campo monto es requerido.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal Monto { get; set; }
    }
}
