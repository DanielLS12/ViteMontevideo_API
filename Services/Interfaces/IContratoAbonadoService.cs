using ViteMontevideo_API.Services.Dtos.Common;
using ViteMontevideo_API.Services.Dtos.ContratosAbonado.Parameters;
using ViteMontevideo_API.Services.Dtos.ContratosAbonado.Requests;
using ViteMontevideo_API.Services.Dtos.ContratosAbonado.Responses;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface IContratoAbonadoService
    {
        Task<DataResponse<ContratoAbonadoResponseDto>> GetAll(int idCajaChica);
        Task<PageCursorMontoResponse<ContratoAbonadoResponseDto>> GetAllPageCursor(FiltroContratoAbonado filtro);
        Task<ContratoAbonadoResponseDto> GetById(int id);
        Task<ApiResponse> Insert(ContratoAbonadoCrearRequestDto abonado);
        Task<ApiResponse> Update(int id, ContratoAbonadoActualizarRequestDto abonado);
        Task<ApiResponse> Pay(int id, ContratoAbonadoPagarRequestDto abonado);
        Task<ApiResponse> CancelPayment(int id);
        Task<ApiResponse> DeleteById(int id);
    }
}
