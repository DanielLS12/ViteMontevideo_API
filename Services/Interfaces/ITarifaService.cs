using ViteMontevideo_API.Services.Dtos.Common;
using ViteMontevideo_API.Services.Dtos.Tarifas.Requests;
using ViteMontevideo_API.Services.Dtos.Tarifas.Responses;

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
