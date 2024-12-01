using ViteMontevideo_API.Services.Dtos.Clientes.Parameters;
using ViteMontevideo_API.Services.Dtos.Clientes.Requests;
using ViteMontevideo_API.Services.Dtos.Clientes.Responses;
using ViteMontevideo_API.Services.Dtos.Common;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface IClienteService
    {
        Task<PageCursorResponse<ClienteResponseDto>> GetAllPageCursor(FiltroCliente filtro);
        Task<ClienteDto> GetById(int id);
        Task<ApiResponse> Insert(ClienteCrearRequestDto cliente);
        Task<ApiResponse> Update(int id, ClienteActualizarRequestDto cliente);
        Task<ApiResponse> DeleteById(int id);
    }
}
