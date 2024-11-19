using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface IActividadRepository : IBaseRepository<short,Actividad>
    {
        Task<bool> ExistsById(short id);
        Task<bool> HasTarifasById(short id);
        Task<bool> ExistsByIdAndNombre(short id,string name);
        Task<bool> ExistsByNombre(string name);
    }
}
