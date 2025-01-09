using ViteMontevideo_API.Services.Dtos.CajasChicas.Parameters;
using ViteMontevideo_API.Services.Dtos.CajasChicas.Requests;
using ViteMontevideo_API.Services.Dtos.CajasChicas.Responses;
using ViteMontevideo_API.Services.Dtos.Common;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface ICajaChicaService
    {
        Task<PageCursorResponse<CajaChicaResponseDto>> GetAllPageCursor(FiltroCajaChica filtro);
        Task<List<InformeCajaChica>> GetAllInformes(DateTime fecha);
        Task<CajaChicaResponseDto> GetById(int id);
        Task<ApiResponse> Open(int id);
        Task<ApiResponse> Close(int id, CajaChicaCerrarRequestDto cajaChicaDto);
        Task<ApiResponse> Insert(CajaChicaCrearRequestDto cajaChicaDto);
        Task<ApiResponse> Update(int id, CajaChicaActualizarRequestDto cajaChicaDto);
        Task<ApiResponse> DeleteById(int id);
    }
}
