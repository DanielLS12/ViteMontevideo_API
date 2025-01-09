using ViteMontevideo_API.Services.Dtos.Common;
using ViteMontevideo_API.Services.Dtos.Egresos.Parameters;
using ViteMontevideo_API.Services.Dtos.Egresos.Requests;
using ViteMontevideo_API.Services.Dtos.Egresos.Responses;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface IEgresoService
    {
        Task<DataResponse<EgresoResponseDto>> GetAll(int idCajaChica);
        Task<PageCursorMontoResponse<EgresoResponseDto>> GetAllPageCursor(FiltroEgreso filtro);
        Task<EgresoResponseDto> GetById(int id);
        Task<ApiResponse> Insert(EgresoCrearRequestDto egreso);
        Task<ApiResponse> Update(int id, EgresoActualizarRequestDto egreso);
        Task<ApiResponse> DeleteById(int id);
    }
}
