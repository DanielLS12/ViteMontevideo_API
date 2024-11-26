using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Presentation.Dtos.ComerciosAdicionales;
using ViteMontevideo_API.Presentation.Dtos.ComerciosAdicionales.Filtros;

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
