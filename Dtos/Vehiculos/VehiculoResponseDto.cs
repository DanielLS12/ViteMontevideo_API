using ViteMontevideo_API.Dtos.Clientes;
using ViteMontevideo_API.Dtos.Tarifas;

namespace ViteMontevideo_API.Dtos.Vehiculos
{
    public class VehiculoResponseDto
    {
        public int IdVehiculo { get; set; }
        public string Placa { get; set; } = null!;
        public virtual TarifaResponseDto OTarifa { get; set; } = null!;
        public virtual ClienteDto? OCliente { get; set; }
    }
}
