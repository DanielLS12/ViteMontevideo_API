using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Services.Dtos.Common;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface ICargoService
    {
        Task<DataResponse<Cargo>> GetAll();
    }
}
