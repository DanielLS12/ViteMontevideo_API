using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Presentation.Dtos.Common;

namespace ViteMontevideo_API.Services.Interfaces
{
    public interface ICargoService
    {
        Task<DataResponse<Cargo>> GetAll();
    }
}
