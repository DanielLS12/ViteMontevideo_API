using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Presentation.Dtos.Servicios;
using ViteMontevideo_API.Presentation.Dtos.Servicios.Filtros;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface IServicioService
    {
        Task<DataResponse<ServicioEntradaResponseDto>> GetAllServiciosEntrada();
        Task<DataMontoResponse<ServicioSalidaResponseDto>> GetAllServiciosSalida(FiltroServicioSalida filtro);
        Task<ServicioEntradaResponseDto> GetServicioEntradaByPlaca(string placa);
        Task<ServicioSalidaResponseDto> GetServicioSalidaById(int id);
        Task<decimal> GetServicioAmount(string placa);
        Task<ApiResponse> Insert(ServicioCrearRequestDto servicio);
        Task<ApiResponse> Update(int id, ServicioActualizarRequestDto servicio);
        Task<ApiResponse> Pay(string placa, ServicioPagarRequestDto servicio);
        Task<ApiResponse> CancelPayment(int id);
        Task<ApiResponse> DeleteById(int id);
    }
}
