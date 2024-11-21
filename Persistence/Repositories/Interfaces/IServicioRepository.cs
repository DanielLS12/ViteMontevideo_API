using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface IServicioRepository : IBaseRepository<int, Servicio>
    {
        Task<bool> HasAnyInProgressByIdVehiculo(int idVehiculo);
    }
}
