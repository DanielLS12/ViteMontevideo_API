using ViteMontevideo_API.Services.Dtos.Common;
using ViteMontevideo_API.Services.Dtos.Usuarios;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<DataResponse<UsuarioBasicResponseDto>> GetAvailableUsers();
        Task<UsuarioResponseDto> GetByUsername(string username);
        Task<ApiResponse> Register(UsuarioRequestDto usuario);
        Task<ApiResponse> Login(UsuarioRequestDto usuario);
    }
}
