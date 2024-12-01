using ViteMontevideo_API.Services.Dtos.Common;
using ViteMontevideo_API.Services.Dtos.Trabajadores.Requests;
using ViteMontevideo_API.Services.Dtos.Trabajadores.Responses;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface ITrabajadorService
    {
        Task<DataResponse<TrabajadorResponseDto>> GetAll();
        Task<TrabajadorResponseDto> GetById(short id);
        Task<ApiResponse> Insert(TrabajadorCrearRequestDto trabajador);
        Task<ApiResponse> Update(short id, TrabajadorActualizarRequestDto trabajador);
        Task<ApiResponse> DeleteById(short id);
    }
}
