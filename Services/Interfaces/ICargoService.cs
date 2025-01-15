using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Services.Dtos.Cargos.Requests;
using ViteMontevideo_API.Services.Dtos.Cargos.Responses;
using ViteMontevideo_API.Services.Dtos.Common;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface ICargoService
    {
        Task<DataResponse<CargoResponseDto>> GetAll();
        Task<ApiResponse> Insert(CargoRequestDto cargo);
        Task<ApiResponse> Update(byte id, CargoRequestDto cargo);
        Task<ApiResponse> DeleteById(byte id);
    }
}
