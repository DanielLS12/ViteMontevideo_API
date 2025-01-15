using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Services.Dtos.Categorias.Requests;
using ViteMontevideo_API.Services.Dtos.Categorias.Responses;
using ViteMontevideo_API.Services.Dtos.Common;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<DataResponse<CategoriaResponseDto>> GetAll();
        Task<CategoriaResponseDto> GetById(short id);
        Task<ApiResponse> Insert(CategoriaRequestDto categoria);
        Task<ApiResponse> Update(short id, CategoriaRequestDto categoria);
        Task<ApiResponse> DeleteById(short id);
    }
}
