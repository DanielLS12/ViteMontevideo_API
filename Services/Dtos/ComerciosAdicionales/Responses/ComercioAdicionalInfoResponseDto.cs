using ViteMontevideo_API.Services.Dtos.Clientes.Responses;

namespace ViteMontevideo_API.Services.Dtos.ComerciosAdicionales.Responses
{
    public class ComercioAdicionalInfoResponseDto
    {
        public decimal Monto { get; set; }
        public ClienteInfoResponseDto Cliente { get; set; } = null!;
    }
}
