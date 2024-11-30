using ViteMontevideo_API.Presentation.Dtos.Tarifas;

namespace ViteMontevideo_API.Presentation.Dtos.Vehiculos
{
    public class VehiculoDetailResponseDto
    {
        public int IdVehiculo { get; set; }
        public string Placa { get; set; } = null!;
        public virtual TarifaResponseDto Tarifa { get; set; } = null!;
    }
}
