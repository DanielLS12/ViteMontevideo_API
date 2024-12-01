using ViteMontevideo_API.Services.Dtos.Tarifas.Responses;

namespace ViteMontevideo_API.Services.Dtos.Vehiculos.Responses
{
    public class VehiculoDetailResponseDto
    {
        public int IdVehiculo { get; set; }
        public string Placa { get; set; } = null!;
        public virtual TarifaResponseDto Tarifa { get; set; } = null!;
    }
}
