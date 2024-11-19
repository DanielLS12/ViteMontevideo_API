using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Presentation.Dtos.Tarifas;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface ITarifaService
    {
        Task<DataResponse<TarifaResponseDto>> GetAll();
        Task<TarifaResponseDto> GetById(short id);
        Task<ApiResponse> Insert(TarifaCrearRequestDto tarifa);
        Task<ApiResponse> Update(short id, TarifaActualizarRequestDto tarifa);
        Task<ApiResponse> DeleteById(short id);
    }
}
