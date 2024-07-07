using System.ComponentModel.DataAnnotations;

namespace ViteMontevideo_API.Dtos.Vehiculos
{
    public class VehiculoActualizarRequestDto
    {
        public short? IdTarifa { get; set; }

        public int? IdCliente { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]{6}$", ErrorMessage = "La placa debe tener exactamente 6 caracteres alfanuméricos.")]
        public string Placa { get; set; } = null!;
    }
}
