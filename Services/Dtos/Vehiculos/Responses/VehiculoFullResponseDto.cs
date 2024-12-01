using ViteMontevideo_API.Services.Dtos.Clientes.Responses;
using ViteMontevideo_API.Services.Dtos.Tarifas.Responses;

namespace ViteMontevideo_API.Services.Dtos.Vehiculos.Responses
{
    public class VehiculoFullResponseDto
    {
        public int IdVehiculo { get; set; }
        public string Placa { get; set; } = null!;
        public virtual TarifaResponseDto Tarifa { get; set; } = null!;
        public virtual ClienteDto? Cliente { get; set; }
    }
}
