using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Presentation.Dtos.ContratosAbonado;
using ViteMontevideo_API.Presentation.Dtos.ContratosAbonado.Filtros;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface IContratoAbonadoService
    {
        Task<PageCursorMontoResponse<ContratoAbonadoResponseDto>> GetAllPageCursor(FiltroContratoAbonado filtro);
        Task<ContratoAbonadoResponseDto> GetById(int id);
        Task<ApiResponse> Insert(ContratoAbonadoCrearRequestDto abonado);
        Task<ApiResponse> Update(int id, ContratoAbonadoActualizarRequestDto abonado);
        Task<ApiResponse> Pay(int id, ContratoAbonadoPagarRequestDto abonado);
        Task<ApiResponse> CancelPayment(int id);
        Task<ApiResponse> DeleteById(int id);
    }
}
