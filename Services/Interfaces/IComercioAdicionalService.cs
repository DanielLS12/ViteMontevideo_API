using ViteMontevideo_API.Services.Dtos.ComerciosAdicionales.Parameters;
using ViteMontevideo_API.Services.Dtos.ComerciosAdicionales.Requests;
using ViteMontevideo_API.Services.Dtos.ComerciosAdicionales.Responses;
using ViteMontevideo_API.Services.Dtos.Common;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface IComercioAdicionalService
    {
        Task<PageCursorMontoResponse<ComercioAdicionalResponseDto>> GetAllPageCursor(FiltroComercioAdicional filtro);
        Task<ComercioAdicionalResponseDto> GetById(int id);
        Task<ApiResponse> Insert(ComercioAdicionalCrearRequestDto comercioAdicional);
        Task<ApiResponse> Update(int id, ComercioAdicionalActualizarRequestDto comercioAdicional);
        Task<ApiResponse> Pay(int id, ComercioAdicionalPagarRequestDto comercioAdicional);
        Task<ApiResponse> CancelPayment(int id);
        Task<ApiResponse> DeleteById(int id);
    }
}
