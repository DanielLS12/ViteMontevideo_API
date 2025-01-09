using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface IEgresoRepository : IBaseRepository<int, Egreso>
    {
        Task<IEnumerable<Egreso>> GetAll(int idCajaChica);
    }
}
