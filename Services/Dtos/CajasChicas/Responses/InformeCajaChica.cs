using ViteMontevideo_API.Services.Dtos.ComerciosAdicionales.Responses;
using ViteMontevideo_API.Services.Dtos.ContratosAbonado.Responses;
using ViteMontevideo_API.Services.Dtos.Egresos.Responses;

namespace ViteMontevideo_API.Services.Dtos.CajasChicas.Responses
{
    public class InformeCajaChica
    {
        public string Cajero { get; set; } = null!;
        public decimal MParticulares { get; set; }
        public decimal MTurnos { get; set; }
        public decimal MEsSalud { get; set; }
        public decimal MEfectivo { get; set; }
        public decimal MYape { get; set; }
        public decimal MOtros { get; set; }
        public decimal MServicios { get; set; }
        public decimal MContratosAbonados { get; set; }
        public decimal MEgresos { get; set; }
        public decimal MComerciosAdicionales { get; set; }
        public ICollection<ComercioAdicionalInfoResponseDto> ComerciosAdicionales { get; set; } = new List<ComercioAdicionalInfoResponseDto>();
        public ICollection<EgresoInfoResponseDto> Egresos { get; set; } = new List<EgresoInfoResponseDto>();
        public ICollection<ContratoAbonadoInfoResponseDto> Abonados { get; set; } = new List<ContratoAbonadoInfoResponseDto>();
    }
}
