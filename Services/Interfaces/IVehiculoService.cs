using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Presentation.Dtos.Vehiculos;
using ViteMontevideo_API.Presentation.Dtos.Vehiculos.Filtros;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface IVehiculoService
    {
        Task<PageCursorResponse<VehiculoFullResponseDto>> GetAllPageCursor(FiltroVehiculo filtro);
        Task<VehiculoFullResponseDto> GetById(int id);
        Task<VehiculoFullResponseDto> GetByPlaca(string placa);
        Task<ApiResponse> Insert(VehiculoCrearRequestDto vehiculo);
        Task<ApiResponse> Update(int id, VehiculoActualizarRequestDto vehiculo);
        Task<ApiResponse> DeleteById(int id);
    }
}
