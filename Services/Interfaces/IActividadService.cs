using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Services.Dtos.Common;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface IActividadService
    {
        Task<DataResponse<Actividad>> GetAll();
        Task<Actividad> GetById(short id);
        Task<ApiResponse> Insert(Actividad actividad);
        Task<ApiResponse> Update(short id, Actividad actividad);
        Task<ApiResponse> DeleteById(short id);
    }
}
