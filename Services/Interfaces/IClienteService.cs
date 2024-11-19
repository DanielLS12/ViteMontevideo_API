using ViteMontevideo_API.Presentation.Dtos.Clientes;
using ViteMontevideo_API.Presentation.Dtos.Clientes.Filtros;
using ViteMontevideo_API.Presentation.Dtos.Common;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface IClienteService
    {
        Task<CursorResponse<ClienteResponseDto>> GetAllPageCursor(FiltroCliente filtro);
        Task<ClienteDto> GetById(int id);
        Task<ApiResponse> Insert(ClienteCrearRequestDto cliente);
        Task<ApiResponse> Update(int id, ClienteActualizarRequestDto cliente);
        Task<ApiResponse> DeleteById(int id);
    }
}
