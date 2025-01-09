using ViteMontevideo_API.Services.Dtos.Vehiculos.Responses;

namespace ViteMontevideo_API.Services.Dtos.ContratosAbonado.Responses
{
    public class ContratoAbonadoInfoResponseDto
    {
        public decimal Monto { get; set; }
        public VehiculoInfoResponseDto Vehiculo { get; set; } = null!;
    }
}
