using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Services.Dtos.Actividades.Requests;
using ViteMontevideo_API.Services.Dtos.Actividades.Responses;
using ViteMontevideo_API.Services.Dtos.Common;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface IActividadService
    {
        Task<DataResponse<ActividadResponseDto>> GetAll();
        Task<ActividadResponseDto> GetById(short id);
        Task<ApiResponse> Insert(ActividadRequestDto actividad);
        Task<ApiResponse> Update(short id, ActividadRequestDto actividad);
        Task<ApiResponse> DeleteById(short id);
    }
}
