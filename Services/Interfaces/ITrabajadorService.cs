using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Presentation.Dtos.Trabajadores;

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
