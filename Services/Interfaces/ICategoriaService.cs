using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Services.Dtos.Common;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<DataResponse<Categoria>> GetAll();
        Task<Categoria> GetById(short id);
        Task<ApiResponse> Insert(Categoria categoria);
        Task<ApiResponse> Update(short id,Categoria categoria);
        Task<ApiResponse> DeleteById(short id);
    }
}
