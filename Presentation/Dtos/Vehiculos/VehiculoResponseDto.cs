using ViteMontevideo_API.Presentation.Dtos.Clientes;
using ViteMontevideo_API.Presentation.Dtos.Tarifas;

namespace ViteMontevideo_API.Presentation.Dtos.Vehiculos
{
    public class VehiculoResponseDto
    {
        public int IdVehiculo { get; set; }
        public string Placa { get; set; } = null!;
        public virtual TarifaResponseDto Tarifa { get; set; } = null!;
        public virtual ClienteDto? Cliente { get; set; }
    }
}
