using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface ITarifaRepository : IBaseRepository<short, Tarifa>
    {
        Task<bool> ExistsById(short id);
        Task<bool> ExistsByCategoriaActividadAndTipo(short categoryId, short activityId, bool isHora);
        Task<bool> HasVehiculosById(short id);
        Task<bool> HasServiciosById(short id);
    }
}
