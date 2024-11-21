using ViteMontevideo_API.Presentation.Dtos.Clientes;
using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Presentation.Dtos.Egresos;
using ViteMontevideo_API.Presentation.Dtos.Egresos.Filtros;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface IEgresoService
    {
        Task<PageCursorMontoResponse<EgresoResponseDto>> GetAllPageCursor(FiltroEgreso filtro);
        Task<EgresoResponseDto> GetById(int id);
        Task<ApiResponse> Insert(EgresoCrearRequestDto egreso);
        Task<ApiResponse> Update(int id, EgresoActualizarRequestDto egreso);
        Task<ApiResponse> DeleteById(int id);
    }
}
